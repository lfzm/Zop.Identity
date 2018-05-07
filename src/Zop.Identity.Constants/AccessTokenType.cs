using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity
{
    /// <summary>
    /// Access token 类型
    /// </summary>
    public enum AccessTokenType
    {
        /// <summary>
        /// Self-contained Json Web Token
        /// </summary>
        Jwt = 0,

        /// <summary>
        /// Reference token
        /// </summary>
        Reference = 1
    }

}
