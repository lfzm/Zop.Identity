using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity
{
    /// <summary>
    /// Token过期类型
    /// </summary>
    public enum TokenExpiration
    {
        /// <summary>
        /// 滑动令牌过期
        /// </summary>
        Sliding = 0,

        /// <summary>
        /// 绝对令牌过期
        /// </summary>
        Absolute = 1
    }
}
