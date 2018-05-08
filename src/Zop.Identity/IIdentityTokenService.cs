using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.DTO;
using Zop.Identity.DTO;

namespace Zop.Identity
{
    /// <summary>
    /// 用户认证令牌
    /// </summary>
    public interface IIdentityTokenService : IGrainWithStringKey
    {
        /// <summary>
        /// 存储发放的认证令牌.
        /// </summary>
        /// <param name="token">token 信息</param>
        /// <returns></returns>
        Task<IdentityTokenAddResponseDto> StoreAsync(IdentityTokenAddRequestDto token);
        /// <summary>
        /// 获取发放的认证令牌
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IdentityTokenDto> GetAsync();
    }
}
