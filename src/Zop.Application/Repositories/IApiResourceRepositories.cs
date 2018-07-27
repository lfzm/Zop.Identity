using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Repositories
{
    /// <summary>
    /// API资源的数据商店
    /// </summary>
    public interface IApiResourceRepositories : IRepository
    {
        /// <summary>
        /// 根据Api资源名称获取Id
        /// </summary>
        /// <param name="name">Api资源名称</param>
        /// <returns></returns>
        Task<int> GetIdAsync(string name);
        /// <summary>
        /// 根据 Api范围名称获取Api范围Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> GetScopeIdAsync(string name);
        /// <summary>
        /// 获取所有的Api资源
        /// </summary>
        /// <returns></returns>
        Task<IList<ApiResource>> GetAllAsync();
        /// <summary>
        /// 根据范围名称获取Api资源
        /// </summary>
        /// <param name="scopeNames">范围名称集合</param>
        /// <returns></returns>
        Task<IList<ApiResource>> GetListAsync(IEnumerable<string> scopeNames);
    }
}
