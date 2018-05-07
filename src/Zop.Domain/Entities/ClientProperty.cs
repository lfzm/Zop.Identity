using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 客户端自定义属性(暂时不知道何用)
    /// </summary>
    [Serializable]
    public class ClientProperty:Entity<int>
    {
        public ClientProperty()
        {

        }
        [JsonConstructor]
        public ClientProperty(string key, string value)
        {
            if (key.IsNull()) throw new ArgumentNullException(nameof(key));
            if (value.IsNull()) throw new ArgumentNullException(nameof(value));
            this.Key = key;
            this.Value = value;
        }
        /// <summary>
        /// 声明类型
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Key { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Value { get; set; }
    }
}
