using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web_Account.Controllers
{
    public class AccountController : Controller
    {
        private readonly OpenIdConnectOptions options;
        private readonly ILogger logger;
        public AccountController(IOptions<OpenIdConnectOptions> _options, ILogger<AccountController> logger)
        {
            this.options = _options?.Value;
            this.logger = logger;
        }
        //登录用户
        public async Task<IActionResult> Index(string return_url)
        {
            //获取客户端　token
            string accessToken = await this.RefreshAccessToken();
            return base.Content(accessToken);
        }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
        public async Task<string> RefreshAccessToken()
        {
            // 从元数据中发现客户端
            DiscoveryClient client = new DiscoveryClient(options.Authority);
            client.Policy.RequireHttps = options.RequireHttpsMetadata;
            var disco = await client.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, options.ClientId, options.ClientSecret);
            var tokenResponseTask = tokenClient.RequestClientCredentialsAsync(string.Join(" ", options.Scope.ToArray()));

            tokenResponseTask.Wait();
            var tokenResponse = tokenResponseTask.Result;
            if (tokenResponse.IsError)
            {
                logger.LogError("ClientCredentials Get AccessToken Error Message:{Error}", tokenResponse.Error);
                return "";
            }
            logger.LogDebug("  AccessToken Response:{Message}", tokenResponse.Json);
            string accessToken = tokenResponse.AccessToken;
            //提前一分钟获取
            var expirationTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn - 60);
            return accessToken;
        }
    }
}