using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    public class Resource : ResponseDto
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 访问令牌中应包含的用户声明类型列表
        /// </summary>
        public ICollection<string> UserClaims { get; set; }
    }
}
