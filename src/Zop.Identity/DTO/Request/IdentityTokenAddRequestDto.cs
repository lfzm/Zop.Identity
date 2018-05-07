using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 身份令牌存储请求Dto
    /// </summary>
    public class IdentityTokenAddRequestDto:RequestDto
    {
        /// <summary>
        /// 认证主题ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string SubjectId { get; set; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ClientId { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Type { get; set; }
        /// <summary>
        /// 认证IP
        /// </summary>
        [Required]
        [IP4]
        public string IdentityIP4 { get; set; }
        /// <summary>
        /// 有效期 默认5分钟有效
        /// </summary>
        public DateTime ValidityTime { get; set; } = DateTime.Now.AddMinutes(5);
        /// <summary>
        /// 返回链接
        /// </summary>
        [MaxLength(1000)]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 认证数据
        /// </summary>
        [Required]
        public Dictionary<string, string> Data { get; private set; } = new Dictionary<string, string>();
    }
}
