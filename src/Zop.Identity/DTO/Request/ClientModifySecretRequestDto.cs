using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端秘钥
    /// </summary>
    public class ClientModifySecretRequestDto : SecretDto
    {
        public ClientModifySecretRequestDto(int id)
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
