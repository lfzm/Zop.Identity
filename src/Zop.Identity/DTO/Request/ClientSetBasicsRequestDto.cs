using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;
using Zop.Identity;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端基本信息和配置Dto
    /// </summary>
    public class ClientSetBasicsRequestDto : RequestDto
    {
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
        /// 是否启用(defaults to <c>true</c>)
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 客户端描述
        /// </summary>
        [MaxLength(20000)]
        public string Description { get; set; }
        /// <summary>
        /// 允许客户端的声明（将包含在访问令牌中）
        /// </summary>
        public Dictionary<string, string> Claims { get; private set; } = new Dictionary<string, string>();

    }
}
