using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
    public class IdentityTokenDto
    {
        /// <summary>
        /// Key
        /// </summary>
        public  string Key { get;  set; }
        /// <summary>
        /// 认证主题ID
        /// </summary>
        public string SubjectId { get; private set; }
        /// <summary>
        /// 认证数据
        /// </summary>
        public Dictionary<string,string> Data { get; set; }
        /// <summary>
        /// 返回链接
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
