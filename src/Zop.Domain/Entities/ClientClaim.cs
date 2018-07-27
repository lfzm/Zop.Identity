using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 客户端声明
    /// </summary>
    [Serializable]
    public class ClientClaim : Entity<int>
    {
        public ClientClaim()
        {

        }
        [JsonConstructor]
        public ClientClaim(string type, string value)
        {
            if (type.IsNull()) throw new ArgumentNullException(nameof(type));
            if (value.IsNull()) throw new ArgumentNullException(nameof(value));
            this.Type = type;
            this.Value = value;
        }
        /// <summary>
        /// 声明类型
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Type { get; private set; }
        [Required]
        [MaxLength(250)]
        public string Value { get; private set; }
    }
}
