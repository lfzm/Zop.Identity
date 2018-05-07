using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace Zop.IdentityCenter.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger _logger;
        public HomeController(IIdentityServerInteractionService interaction, ILogger<HomeController> logger)
        {
            this._interaction = interaction;
            this._logger = logger;
        }
        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                this._logger.LogError("Error=>" + message.ToJsonString());
                return base.Content(message.Error);
            }
            else
                return base.Content("认证中心-认证失败");
        }
    }
}