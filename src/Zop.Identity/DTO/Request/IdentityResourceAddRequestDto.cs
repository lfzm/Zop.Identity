using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 添加认证资源Dto
    /// </summary>
   public class IdentityResourceAddRequestDto: IdentityResourceModifyRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        public List<string> UserClaims { get; private set; } = new List<string>();
    }
}
