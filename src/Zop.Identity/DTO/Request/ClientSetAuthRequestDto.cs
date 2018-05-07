using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;
using Zop.Identity;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端认证配置Dto
    /// </summary>
    public class ClientSetAuthRequestDto : RequestDto
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        [MaxLength(300)]
        public string LoginUri { get; set; }
        /// <summary>
        /// 客户端允许使用的授予类型
        /// </summary>
        public List<string> AllowedGrantTypes { get; private set; } = new List<string>();
        /// <summary>
        /// 客户端访问范围资源
        /// </summary>
        public List<string> AllowedScopes { get; private set; } = new List<string>();
        /// <summary>
        /// 允许返回令牌或授权码的URI
        /// </summary>
        public List<string> RedirectUris { get; private set; } = new List<string>();

        /// <summary>
        /// 指定此客户端是否可以使用本地帐户，或仅使用外部IdP。默认为true。
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;
        /// <summary>
        /// 客户端可以用哪些外部IdP（如果列表为空，则允许所有IdP）。默认为空。
        /// </summary>
        public List<string> IdentityProviderRestrictions { get; private set; } = new List<string>();
        /// <summary>
        /// 授权协议类型
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProtocolType { get; set; } = ProtocolTypes.OpenIdConnect;
        /// <summary>
        /// 允许CORS策略
        /// </summary>
        public List<string> AllowedCorsOrigins { get; private set; } = new List<string>();
    }
}
