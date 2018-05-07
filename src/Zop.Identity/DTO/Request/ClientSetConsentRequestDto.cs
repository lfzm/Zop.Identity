using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端同意页面配置Dto
    /// </summary>
    public class ClientSetConsentRequestDto : RequestDto
    {
        /// <summary>
        /// 是否需要同意屏幕（默认为true）
        /// </summary>
        public bool RequireConsent { get; set; } = true;
        /// <summary>
        /// 用户是否可以选择存储同意决定（默认为true）
        /// </summary>
        public bool AllowRememberConsent { get; set; } = true;
        /// <summary>
        /// 用户同意的生命周期。默认为空（不过期）(秒)。
        /// </summary>
        public int? ConsentLifetime { get; set; } = null;
    }
}
