using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 修改Api资源中的范围信息Dto
    /// </summary>
    public class ApiResourceModifyScopeRequestDto : ScopeDto
    {
        public ApiResourceModifyScopeRequestDto(string  name)
        {
            if (name.IsNull())
                throw new ArgumentNullException("Api范围名称不能为空");
            this.Name = name;
        }

    }
}
