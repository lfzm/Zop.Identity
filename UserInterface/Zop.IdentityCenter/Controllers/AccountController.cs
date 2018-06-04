using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Events;
using Zop.DTO;
using Zop.Identity.DTO;
using Zop.IdentityCenter.Application;
using Zop.IdentityCenter.Configuration;
using Microsoft.Extensions.Options;

namespace Zop.IdentityCenter.Controllers
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly ILogger logger;
        private readonly IAccountService service;
        public AccountController(ILogger<AccountController> logger,IAccountService service)
        {
            this.logger = logger;
            this.service = service;
        }

        public IActionResult Index()
        {
            //默认跳转官方网站
            return base.Content("跳转官网");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (returnUrl.IsNull())
                return base.RedirectToAction("index");

            
            string loginUrl = await service.Login(returnUrl);
            return base.Redirect(loginUrl);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "idc")]
        public Task<IdentityTokenAddResponseDto> Login([FromBody]IdentityTokenAddRequestDto dto)
        {
            return service.Login(dto);
        }
        [HttpGet]
        public async  Task<IActionResult> LoginCallback(string token)
        {
            string loginUrl = await service.LoginCallback(token);
            return base.Redirect(loginUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            string logoutUrl = await service.Logout(logoutId);
            if (logoutUrl.IsNull())
               return base.Content("退出返回地址不能为空");
            return base.Redirect(logoutUrl);
        }
    }
}