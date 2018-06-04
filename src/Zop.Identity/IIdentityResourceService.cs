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
    /// 认证资源服务
    /// </summary>
    public interface IIdentityResourceService : IGrainWithIntegerKey
    {
        /// <summary>
        /// 根据scope name 获取身份认证资源
        /// </summary>
        /// <param name="scopeNames">资源名称</param>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<IEnumerable<IdentityResourceDto>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames);
        /// <summary>
        /// 获取所有的身份认证资源
        /// </summary>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<IEnumerable<IdentityResourceDto>> GetAllAsync();
        /// <summary>
        /// 获取身份认证资源
        /// </summary>
        /// <returns></returns>
        Task<IdentityResourceDto> GetAsync();
        /// <summary>
        /// 添加认证资源信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> AddAsync(IdentityResourceAddRequestDto dto);
        /// <summary>
        /// 修改认证资源信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> Modify(IdentityResourceModifyRequestDto dto);
        /// <summary>
        /// 修改认证资源声明信息
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetClaims(List<string> claims);

        
    }
}
