using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Identity.DTO;
using Zop.IdentityCenter.DTO;

namespace Zop.IdentityCenter.Application
{
    public interface IAccountService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="returnUrl">登录成功返回链接</param>
        /// <returns></returns>
        Task<string> Login(string returnUrl);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto">登录请求</param>
        /// <returns></returns>
        Task<IdentityTokenAddResponseDto> Login(LoginRequestDto dto);
        /// <summary>
        /// 登录回调
        /// </summary>
        /// <param name="token">登录令牌</param>
        /// <returns></returns>
        Task<string> LoginCallback(string token);
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        Task<string> Logout(string logoutId);
    }
}
