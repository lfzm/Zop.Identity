using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Zop.Identity;
using Zop.Toolkit.Security;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 秘钥
    /// </summary>
    [Serializable]
    public class Secret:Entity<int>
    {
        #region 构造函数
        /// <summary>
        /// 初始化 <see cref="ApiSecret"/>
        /// </summary>
        public Secret()
        {
            Type = SecretTypes.SharedSecret;
        }

        /// <summary>
        /// 初始化 <see cref="ApiSecret"/> 
        /// </summary>
        /// <param name="value">秘钥</param>
        /// <param name="type">秘钥类型</param>
        /// <param name="expiration">过期时间</param>
        public Secret( string value, string type = SecretTypes.SharedSecret, DateTime? expiration = null)
            : this(value, type, "", expiration)
        {

        }

        /// <summary>
        /// 初始化 <see cref="ApiSecret"/> 
        /// </summary>
        /// <param name="value">秘钥</param>
        /// <param name="type">秘钥类型</param>
        /// <param name="description">描述</param>
        /// <param name="expiration">过期时间</param>
        public Secret( string value, string type = SecretTypes.SharedSecret, string description="", DateTime? expiration = null)
            : this()
        {
            Description = description;
            Expiration = expiration;
            if (type == SecretTypes.SharedSecret)
            {
                Value = value.Sha256();
            }
        }
        #endregion

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
        /// 密钥类型 SecretTypes
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }

        
    }
}
