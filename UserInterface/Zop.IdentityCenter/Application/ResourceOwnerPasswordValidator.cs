using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Zop.Extensions.OrleansClient;

namespace Zop.IdentityCenter.Application
{
    /// <summary>
    /// Password 模式验证服务
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IOrleansClient client;
        private readonly ILogger logger;
        public ResourceOwnerPasswordValidator(IOrleansClient client, ILogger<ResourceOwnerPasswordValidator> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据账户寻找认证数据
            var service = client.GetGrain<Zop.Identity.IIdentityTokenService>(context.UserName);
            var token = await service.GetAsync();
            if (token == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                this.logger.LogError($"密码模式验证失败：{context.UserName}-{context.Password}；授权Token不存在或者失效");
                return;
            }
            if (token.IdentityIP4 == "::1")
                token.IdentityIP4 = "127.0.0.1";
            if (token.IdentityIP4 != context.Password)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                this.logger.LogError($"密码模式验证失败：{context.UserName}-{context.Password}；授权无效");
                return;
            }
            ///用户声明
            List<Claim> claims = Mapper.Map<List<Claim>>(token.Data);
            context.Result = new GrantValidationResult(token.SubjectId, "pwd", DateTime.Now, claims);
            return;
        }
    }
}
