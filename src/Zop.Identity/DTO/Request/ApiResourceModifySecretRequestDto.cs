using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Zop.Identity.DTO
{
    /// <summary>
    /// 修改Api资源中的秘钥信息Dto
    /// </summary>
   public class ApiResourceModifySecretRequestDto: SecretDto
    {
        public ApiResourceModifySecretRequestDto(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("Api范围Id不能为空");
            this.Id = id;
        }
        /// <summary>
        /// 范围Id
        /// </summary>
        [Required]
        public int Id { get; }
    }
}
