using System;
using System.Collections.Generic;
using System.Text;
using Orleans;

namespace Zop.Identity
{
    /// <summary>
    /// Token过期清除服务
    /// </summary>
    public interface ITokenExpiredCleanupService : IGrainWithGuidKey
    {
        
    }
}
