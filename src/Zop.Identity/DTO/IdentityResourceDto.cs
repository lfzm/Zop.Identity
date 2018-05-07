using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity.DTO
{
    public class IdentityResourceDto: Resource
    {
        /// <summary>
        /// 是否可以在同意页面上取消选择范围
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 是否在同意页面强调此范围
        /// </summary>
        public bool Emphasize { get; set; }
        /// <summary>
        /// 指定此范围是否显示在发现文档中
        /// </summary>
        public bool ShowInDiscoveryDocument { get; set; }

    }
}
