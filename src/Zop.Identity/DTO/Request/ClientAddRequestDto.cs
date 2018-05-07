using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 客户端添加请求Dto
    /// </summary>
    public class ClientAddRequestDto : RequestDto
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string MerchantId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [MaxLength(200)]
        public string ClientName { get; set; }
        /// <summary>
        /// 有关客户信息的URI
        /// </summary>
        [MaxLength(2000)]
        public string ClientUri { get; set; }
        /// <summary>
        /// Logo Uri
        /// </summary>
        [MaxLength(2000)]
        public string LogoUri { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        [MaxLength(300)]
        public string LoginUri { get; set; }
        /// <summary>
        /// 客户端描述
        /// </summary>
        [MaxLength(20000)]
        public string Description { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public List<SecretDto> Secrets { get; private set; } = new List<SecretDto>();
        /// <summary>
        /// 客户端允许使用的授予类型。使用GrantTypes该类来进行常见组合
        /// </summary>
        public List<string> AllowedGrantTypes { get; private set; } = new List<string>();
        /// <summary>
        /// 客户端访问范围资源
        /// </summary>
        public List<string > AllowedScopes { get; private set; } = new List<string>();
    }
}
