using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 添加Api资源的请求Dto
    /// </summary>
    public class ApiResourceAddRequestDto:RequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [MaxLength(200)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        public List<string> UserClaims { get; private set; } = new List<string>();
        /// <summary>
        /// 密钥
        /// </summary>
        [CollectionCount(minCount: 1)]
        public IList<SecretDto> Secrets { get; private set; } = new List<SecretDto>();
        /// <summary>
        /// 包含范围
        /// </summary>
        public IList<ScopeDto> Scopes { get; private set; } = new List<ScopeDto>();
    }
}
