using IdentityServer4.Services;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zop.IdentityCenter.Application
{
    /// <summary>
    /// 客户端服务
    /// </summary>
    public interface IClientService : IClientStore, ICorsPolicyService
    {
    }
}
