using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    public class PersistedGrantDto:RequestDto
    {
        /// <summary>
        /// Key
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Key { get; set; }
        /// <summary>
        /// 令牌类型
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Type { get; set; }
        /// <summary>
        /// 主题Id
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
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// 令牌数据
        /// </summary>
        [Required]
        [MaxLength(50000)]
        public string Data { get; set; }
    }
}
