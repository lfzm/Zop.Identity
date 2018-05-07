using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.Identity;

namespace Zop.Domain.Entities
{
    ///<summary>
    ///客户端权限范围
    ///</summary>
    [Serializable]
    public class ClientSecret:Entity<int>
    {
        #region 构造函数
        /// <summary>
        /// 初始化 <see cref="ClientSecret"/>
        /// </summary>
        public ClientSecret()
        {
            Type = SecretTypes.SharedSecret;
        }

        /// <summary>
        /// 初始化 <see cref="ClientSecret"/> 
        /// </summary>
        /// <param name="value">秘钥</param>
        /// <param name="expiration">过期时间</param>
        public ClientSecret(string value, DateTime? expiration = null)
            : this()
        {
            Value = value;
            Expiration = expiration;
        }

        /// <summary>
        /// 初始化 <see cref="ClientSecret"/> 
        /// </summary>
        /// <param name="value">秘钥</param>
        /// <param name="description">描述</param>
        /// <param name="expiration">过期时间</param>
        public ClientSecret(string value, string description, DateTime? expiration = null)
            : this()
        {
            Description = description;
            Value = value;
            Expiration = expiration;
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
        /// 密钥类型
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }
       
    }
}
