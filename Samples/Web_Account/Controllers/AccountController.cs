using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web_Account.Models;
using Newtonsoft.Json;

namespace Web_Account.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger logger;
        public AccountController(IOptions<OpenIdConnectOptions> _options, ILogger<AccountController> logger)
        {

            this.logger = logger;
        }
        //登录用户
        public async Task<IActionResult> Index(string return_url)
        {

            var request = new IdentityTokenAddRequestDto()
            {
                ClientId = "164965530879528960",
                IdentityIP4 = "127.0.0.1",
                SubjectId = "1",
                Type = "用户登录",
                ReturnUrl = return_url
            };
            request.Claims.Add("sud", "1");
            request.Claims.Add("phone", "18165445656");
            request.Claims.Add("nickname", "测试");

            //获取客户端　token
            string accessToken = await this.RefreshAccessToken();
            string content = JsonConvert.SerializeObject( request);

            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            //创建一个HttpClient
            HttpClient client = new HttpClient(handler);
            client.SetBearerToken(accessToken);
            //创建一个HttpContent
            HttpContent httpContent = new StringContent(content,Encoding.UTF8);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = await client.PostAsync("http://localhost:5000/account/login", httpContent);
            var msg = await responseMessage.Content.ReadAsStringAsync();
            var r = JsonConvert.DeserializeObject<IdentityTokenAddResponseDto>(msg);
            return base.Redirect($"http://localhost:5000/account/LoginCallback?token={r.Token}");
        }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
        public async Task<string> RefreshAccessToken()
        {
            // 从元数据中发现客户端
            DiscoveryClient client = new DiscoveryClient("http://localhost:5000/");
            client.Policy.RequireHttps =false;
            var disco = await client.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, "164965530879528960", "secret");
            var tokenResponseTask = tokenClient.RequestClientCredentialsAsync("IDC_API");

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