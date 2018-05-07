using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端注销登录的配置请求Dto
    /// </summary>
    public class ClientSetLogoutRequestDto : RequestDto
    {
        /// <summary>
        /// 指定基于HTTP的前台通道注销的注销URI
        /// </summary>
        [MaxLength(2000)]
        public string FrontChannelLogoutUri { get; set; }
        /// <summary>
        /// 指定是否应将用户的会话标识发送到FrontChannelLogoutUri。默认为true。
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// 指定基于HTTP的反向通道注销的注销URI
        /// </summary>
        [MaxLength(2000)]
        public string BackChannelLogoutUri { get; set; }
        /// <summary>
        /// 指定是否在请求中将用户的会话ID发送到BackChannelLogoutUri。默认为true。
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// 允许注销后重定向到URI
        /// </summary>
        public List<string> PostLogoutRedirectUris { get; private set; } = new List<string>();
    }
}
