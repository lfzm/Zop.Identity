using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.DataStore
{
    /// <summary>
    /// 发放的令牌数据商店
    /// </summary>
    public interface IPersistedGrantDataStore : IDataStore
    {
        /// <summary>
        /// 根据SubjectId获取令牌的Key
        /// </summary>
        /// <param name="subjectId">subjectId</param>
        /// <returns></returns>
        Task<IList<string>> GetKeysAsync(string subjectId);
        /// <summary>
        /// 获取发放的令牌key
        /// </summary>
        /// <param name="subjectId">授权唯一标识</param>
        /// <param name="clientId">客户端Id</param>
        /// <returns></returns>
        Task<IList<string>> GetKeysAsync(string subjectId, string clientId);
        /// <summary>
        /// 获取发放的令牌key
        /// </summary>
        /// <param name="subjectId">授权唯一标识</param>
        /// <param name="clientId">客户端Id</param>
        /// <param name="type">令牌类型</param>
        /// <returns></returns>
        Task<IList<string>> GetKeysAsync(string subjectId, string clientId, string type);
        /// <summary>
        /// 根据subjectId获取发放的令牌
        /// </summary>
        /// <param name="subjectId">subjectId</param>
        /// <returns></returns>
        Task<IList<PersistedGrant>> GetListAsync(string subjectId);


    }
}
