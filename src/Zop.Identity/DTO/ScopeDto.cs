using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    public class ScopeDto:RequestDto
    {
        /// <summary>
        /// 范围名称. 客户端用来请求范围.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称. 用于显示在同意页面上
        /// </summary>
        [MaxLength(200)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述. 用于显示在同意页面上
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 是否可以在同意页面上取消选择范围
        /// </summary>
        public bool Required { get; set; } = false;
        /// <summary>
        /// 是否在同意页面强调此范围
        /// </summary>
        public bool Emphasize { get; set; } = false;
        /// <summary>
        /// 指定此范围是否显示在发现文档中
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        public ICollection<string> UserClaims { get; set; } = new List<string>();
    }
}
