using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Application.DataStore
{
    /// <summary>
    /// 客户端数据商店
    /// </summary>
   public interface IClientDataStore:IDataStore
    {

        /// <summary>
        /// 确定CORS策略是否允许来源
        /// </summary>
        /// <param name="origin">安全策略</param>
        /// <returns></returns>
        Task<bool> IsOriginAllowedAsync(string origin);
    }
}
