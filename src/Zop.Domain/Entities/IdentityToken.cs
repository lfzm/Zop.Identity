using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 身份令牌
    /// </summary>
    [Serializable]
    public class IdentityToken : Entity<string>
    {
        public IdentityToken() { }
        public IdentityToken(string id,string subjectId)
        {
            this.SetId( id);
            this.SubjectId = subjectId;
        }
        /// <summary>
        /// Key
        /// </summary>
        [Required]
        [MaxLength(200)]
        [JsonProperty]
        public override string Id { get; protected set; }
        /// <summary>
        /// 认证主题ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        [JsonProperty]
        public string SubjectId { get; private set; }
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
        public string Type { get;  set; }
        /// <summary>
        /// 认证IP
        /// </summary>
        [Required]
        [IP4]
        public string IdentityIP4 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ValidityTime { get; set; }
        /// <summary>
        /// 返回链接
        /// </summary>
        [MaxLength(1000)]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 认证数据
        /// </summary>
        [Required]
        [MaxLength(50000)]
        public string Data { get;  set; }
    }
}
