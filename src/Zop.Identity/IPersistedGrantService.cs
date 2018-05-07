using System;
using System.Collections.Generic;
using System.Text;
using Orleans;
using System.Threading.Tasks;
using Zop.Identity.DTO;
using Zop.DTO;

namespace Zop.Identity
{
    /// <summary>
    /// 发放的令牌
    /// </summary>
    public interface IPersistedGrantService : IGrainWithStringKey
    {
        /// <summary>
        /// 获取发放的令牌
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<PersistedGrantDto> GetAsync();
        /// <summary>
        /// 获取所有发放的令牌
        /// </summary>
        /// <param name="subjectId">主题标识符</param>
        /// <returns></returns>
        Task<IList<PersistedGrantDto>> GetAllAsync(string subjectId);
        /// <summary>
        /// 存储发放的令牌.
        /// </summary>
        /// <param name="grant">The grant.</param>
        /// <returns></returns>
        Task<ResultResponseDto> StoreAsync(PersistedGrantDto grant);
        /// <summary>
        /// 根据Key移除发放的令牌
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveAsync();
        /// <summary>
        /// 删除给定主题ID和客户端ID组合的所有授权
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveAllAsync(string subjectId, string clientId);
        /// <summary>
        /// 删除给定主题ID和客户端ID组合的所有授予类型
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveAllAsync(string subjectId, string clientId, string type);

    }
}
