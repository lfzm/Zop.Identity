using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity.DTO
{
    public class ClaimDto
    {
        /// <summary>
        /// 声明类型
        /// </summary>
        public string Type { get; private set; }


        public string Value { get; private set; }
    }
}
