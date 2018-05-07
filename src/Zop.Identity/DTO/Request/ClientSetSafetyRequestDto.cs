using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端安全配置Dto
    /// </summary>
    public class ClientSetSafetyRequestDto:RequestDto
    {
        /// <summary>
        /// 是否需要密钥来从令牌端点请求令牌（默认为true）
        /// </summary>
        public bool RequireClientSecret { get; set; } = true;
        /// <summary>
        /// 指定使用基于授权代码的授权类型的客户端是否必须发送证明密钥 (defaults to <c>false</c>).
        /// </summary>
        public bool RequirePkce { get; set; } = false;
        /// <summary>
        /// 使用PKCE的客户端是否可以使用纯文本代码（不推荐 - 并且默认为false）
        /// </summary>
        public bool AllowPlainTextPkce { get; set; } = false;

    }
}
