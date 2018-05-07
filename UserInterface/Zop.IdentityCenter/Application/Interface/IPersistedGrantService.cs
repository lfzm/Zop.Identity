using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zop.IdentityCenter.Application
{
    /// <summary>
    /// 发行令牌
    /// </summary>
    public interface IPersistedGrantService : IPersistedGrantStore
    {
    }
}
