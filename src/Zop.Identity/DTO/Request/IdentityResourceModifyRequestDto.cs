using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
   public class IdentityResourceModifyRequestDto : RequestDto
    {
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
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 是否可以在同意页面上取消选择范围。 默认为false。
        /// </summary>
        public bool Required { get; set; } = false;
        /// <summary>
        /// 是否在同意页面强调此范围。 默认为false。
        /// </summary>
        public bool Emphasize { get; set; } = false;
        /// <summary>
        /// 是否显示在发现文档中
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; } = true;
    }
}
