using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity.DTO
{
    public class ApiResourceDto: Resource
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public ICollection<SecretDto> ApiSecrets { get; set; }
        /// <summary>
        /// 包含范围
        /// </summary>
        public ICollection<ScopeDto> Scopes { get; set; }
    }
}
