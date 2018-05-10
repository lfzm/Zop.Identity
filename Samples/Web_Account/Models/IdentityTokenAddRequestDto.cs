using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Account.Models
{
    public class IdentityTokenAddRequestDto
    {
        /// <summary>
        /// 认证主题ID
        /// </summary>

        public string SubjectId { get; set; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 认证IP
        /// </summary>
        public string IdentityIP4 { get; set; }
        /// <summary>
        /// 有效期 默认5分钟有效
        /// </summary>
        public DateTime ValidityTime { get; set; } = DateTime.Now.AddMinutes(5);
        /// <summary>
        /// 返回链接
        /// </summary>

        public string ReturnUrl { get; set; }
        /// <summary>
        /// 认证数据
        /// </summary>

        public Dictionary<string, string> Claims { get; private set; } = new Dictionary<string, string>();
    }
}
