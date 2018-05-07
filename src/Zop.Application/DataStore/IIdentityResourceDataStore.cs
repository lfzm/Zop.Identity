using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.DataStore
{
    /// <summary>
    /// 认证资源数据商店
    /// </summary>
    public interface IIdentityResourceDataStore : IDataStore
    {
        /// <summary>
        /// 根据认证资源名称获取Id
        /// </summary>
        /// <param name="name">Api资源名称</param>
        /// <returns></returns>
        Task<int> GetIdAsync(string name);
        /// <summary>
        /// 获取所有的认证资源
        /// </summary>
        /// <returns></returns>
        Task<IList<IdentityResource>> GetAllAsync();
        /// <summary>
        /// 根据范围名称获取认证资源
        /// </summary>
        /// <param name="scopeNames">范围名称集合</param>
        /// <returns></returns>
        Task<IList<IdentityResource>> GetListAsync(IEnumerable<string> scopeNames);
    }
}
