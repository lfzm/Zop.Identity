using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 修改Api资源信息Dto
    /// </summary>
    public class ApiResourceModifyRequestDto : RequestDto
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; } = true;
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

    }
}
