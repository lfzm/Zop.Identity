using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    public class SecretDto:RequestDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecretDto"/> class.
        /// </summary>
        public SecretDto()
        {
            Type = SecretTypes.SharedSecret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretDto"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expiration">The expiration.</param>
        public SecretDto(string value, DateTime? expiration = null)
            : this()
        {
            Value = value;
            Expiration = expiration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretDto" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="description">The description.</param>
        /// <param name="expiration">The expiration.</param>
        public SecretDto(string value, string description, DateTime? expiration = null)
            : this()
        {
            Description = description;
            Value = value;
            Expiration = expiration;
        }
      
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        [MaxLength(10000)]
        public string Value { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// 密钥类型
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }

    }
}
