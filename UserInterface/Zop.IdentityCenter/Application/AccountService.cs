using AutoMapper;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zop.DTO;
using Zop.Identity.DTO;
using Zop.IdentityCenter.Configuration;
using Zop.IdentityCenter.DTO;
using Zop.OrleansClient;

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
          IEventService events,  IOptions<IdentityCenterOptions> options, 
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
            //获取客户端默认的登陆链接
            string loginUrl = options.DefaultLoginUrl;

            var VisitorTerminal = httpContextAccessor.HttpContext.GetVisitorTerminal();
            string terminal = VisitorTerminal.Terminal.ToString();
            if (loginUrl.Contains("?"))
                return $"{loginUrl}&return_url={returnUrl}&terminal={terminal}";
            else
                return $"{loginUrl}?return_url={returnUrl}&terminal={terminal}";
        }

        public Task<IdentityTokenAddResponseDto> Login(LoginRequestDto dto)
        {
            Result result = dto.ValidResult();
            if (!result.Success)
            {
                Task.FromResult(Result.ReFailure<IdentityTokenAddResponseDto>(result));
            }
            dto.ClientId = httpContextAccessor.HttpContext.User.UserId().ToString();
            var service = client.GetGrain<Zop.Identity.IIdentityTokenService>(Guid.NewGuid().ToString());
            return service.StoreAsync(dto);
        }

        public async Task<string> LoginCallback(string key)
        {
            //根据token获取登录信息
            var service = client.GetGrain<Zop.Identity.IIdentityTokenService>(key);
            var token = await service.GetAsync();
            if (token == null)
                throw new ZopException(key + "授权Token不存在或者失效 ");

            //登录成功
            await this.events.RaiseAsync(new UserLoginSuccessEvent(token.SubjectId, token.SubjectId, token.SubjectId));
            //处理记住登录状态
            AuthenticationProperties props = null;
            if (this.options.AllowRememberLogin )
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
                //删除本地认证cookie
                await this.httpContextAccessor.HttpContext.SignOutAsync();
                // 触发注销事件
                await this.events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()));
            }
            return logoutRequest.PostLogoutRedirectUri;
        }
    }
}
