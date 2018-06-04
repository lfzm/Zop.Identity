using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Identity.DTO;
using Zop.DTO;
using Orleans.Concurrency;

namespace Zop.Identity
{
    /// <summary>
    /// API资源服务
    /// </summary>
    public interface IApiResourceService : IGrainWithIntegerKey
    {
        /// <summary>
        /// 根据 scope name获取API资源
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<IEnumerable<ApiResourceDto>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames);
        /// <summary>
        /// 根据name获取API资源 
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<ApiResourceDto> FindApiResourceAsync(string name);
        /// <summary>
        /// 获取所有的API资源 
        /// </summary>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<IEnumerable<ApiResourceDto>> GetAllAsync();
        /// <summary>
        /// 获取API资源 
        /// </summary>
        /// <returns></returns>
        [AlwaysInterleave]
        Task<ApiResourceDto> GetAsync();
        /// <summary>
        /// 添加API资源
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> AddAsync(ApiResourceAddRequestDto dto);
        /// <summary>
        /// 添加秘钥
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> AddSecret(SecretDto dto);
        /// <summary>
        /// 添加Api范围
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> AddScope(ScopeDto dto);
        /// <summary>
        /// 修改Api资源中的秘钥信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> ModifySecret(ApiResourceModifySecretRequestDto dto);
        /// <summary>
        /// 修改Api资源中的范围信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> ModifyScope(ApiResourceModifyScopeRequestDto dto);
        /// <summary>
        /// 修改Api资源声明信息
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<ResultResponseDto> ModifyClaims(List<string> claims);
        /// <summary>
        /// 修改Api资源信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> Modify(ApiResourceModifyRequestDto dto);
        /// <summary>
        /// 根据Name移除Api范围
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveScope(string name);
        /// <summary>
        /// 根据ID移除秘钥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveSecret(int id);
    }
}
