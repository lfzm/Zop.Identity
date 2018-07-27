using AutoMapper;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Zop.Extensions.OrleansClient;
using Zop.IdentityCenter.Configuration;

namespace Zop.IdentityCenter.Application
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOrleansClient client;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IdentityCenterOptions options;
        private readonly ILogger logger;
        public AccountService(IOrleansClient client, ILogger<AccountService> logger, IIdentityServerInteractionService interaction,
          IEventService events, IOptions<IdentityCenterOptions> options,
          IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.logger = logger;
            this.interaction = interaction;
            this.events = events;
            this.httpContextAccessor = httpContextAccessor;
            this.options = options?.Value;
        }


        public async Task<string> Login(string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            if (context == null)
            {
                this.logger.LogDebug("登录参数不合法；" + returnUrl);
                return options.DefaultLoginUrl;
            }

            var service = client.GetGrain<Zop.Identity.IClientService>(context.ClientId);
            //获取客户端默认的登陆链接
            string loginUrl = await service.GetLoginUrlAsync();
            if (loginUrl.IsNull())
                loginUrl = options.DefaultLoginUrl;

            if (!loginUrl.Contains("?"))
                loginUrl += "?";
            return $"{loginUrl}return_url={WebUtility.UrlEncode(returnUrl)}";
        }


        public async Task<string> LoginCallback(string key)
        {
            //根据token获取登录信息
            var service = client.GetGrain<Zop.Identity.IIdentityTokenService>(key);
            var token = await service.GetAsync();
            if (token == null)
                throw new ZopException(key + "授权Token不存在或者失效 ");
            this.logger.LogDebug("token-> {token}", token);

            string clientIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            if (clientIp == "::1")  clientIp = "127.0.0.1";
            if (token.IdentityIP4 != clientIp)
                throw new ZopException(key + "授权无效");

            //登录成功
            await this.events.RaiseAsync(new UserLoginSuccessEvent(token.SubjectId, token.SubjectId, token.SubjectId));
            //处理记住登录状态
            AuthenticationProperties props = null;
            if (this.options.AllowRememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(this.options.RememberLoginLifetime)
                };
            };
            ///用户声明
            List<Claim> claims = Mapper.Map<List<Claim>>(token.Data);
            // 登录信息保存到Cookie中
            await httpContextAccessor.HttpContext.SignInAsync(token.SubjectId, "", props, claims.ToArray());
            return token.ReturnUrl;
        }

        public async Task<string> Logout(string logoutId)
        {
            //获取登出请求对象
            var logoutRequest = await this.interaction.GetLogoutContextAsync(logoutId);
            //获取登录输出对象
            this.logger.LogDebug("{logoutId} logout {logoutRequest}", logoutId, logoutRequest.ToJsonString());
            var user = this.httpContextAccessor.HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                //获取外部授权提供商
                //var idp = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                //if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                //{
                //    //外部授权提供商是否支持注销
                //    var providerSupportsSignout = await httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(idp);
                //    if (providerSupportsSignout)
                //    {
                //        if (logoutId == null)      //注销上下文为空
                //        {
                //            // if there's no current logout context, we need to create one
                //            // this captures necessary info from the current logged in user
                //            // before we signout and redirect away to the external IdP for signout
                //            logoutId = await interaction.CreateLogoutContextAsync();
                //        }
                //        dto.ExternalAuthenticationScheme = idp;
                //    }
                //}
                this.logger.LogDebug("SignOut Success");
                //删除本地认证cookie
                await this.httpContextAccessor.HttpContext.SignOutAsync();
                // 触发注销事件
                await this.events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
            }
            return logoutRequest.PostLogoutRedirectUri;
        }
    }
}
