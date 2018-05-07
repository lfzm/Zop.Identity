using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO
{
   public class IdentityTokenAddResponseDto:ResultResponseDto
    {
        public IdentityTokenAddResponseDto() { }
        public IdentityTokenAddResponseDto(string token) : base()
        {
            this.Token = token;
        }
        /// <summary>
        /// 身份令牌
        /// </summary>
        public string Token { get; }
    }
}
