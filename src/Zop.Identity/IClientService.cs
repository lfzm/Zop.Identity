using System;
using System.Collections.Generic;
using System.Text;
using Orleans;
using System.Threading.Tasks;
using Zop.Identity.DTO;
using Zop.DTO;
using Zop.Identity.DTO.Request;

namespace Zop.Identity
{
    /// <summary>
    /// 客户端服务
    /// </summary>
    public interface IClientService : IGrainWithStringKey
    {
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns></returns>
        Task<ClientDto> GetAsync();
        /// <summary>
        /// 确定CORS策略是否允许来源
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        Task<bool> IsOriginAllowedAsync(string origin);

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> AddAsync(ClientAddRequestDto dto);
        /// <summary>
        /// 设置客户端授权配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetAuthConfig(ClientSetAuthRequestDto dto);
        /// <summary>
        /// 设置客户端基本信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetBasics(ClientSetBasicsRequestDto dto);
        /// <summary>
        /// 设置客户端同意授权配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetConsentConfig(ClientSetConsentRequestDto dto);
        /// <summary>
        /// 设置客户端注销登录配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetLogoutConfig(ClientSetLogoutRequestDto dto);
        /// <summary>
        /// 设置客户端授权安全配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetSafetyConfig(ClientSetSafetyRequestDto dto);
        /// <summary>
        /// 设置客户端授权Token配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> SetTokenConfig(ClientSetTokenRequestDto dto);
        /// <summary>
        /// 设置客户端的密钥
        /// </summary>
        Task<ResultResponseDto> AddSecrets(SecretDto secrets);
        /// <summary>
        /// 移除客户端的秘钥
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultResponseDto> RemoveSecrets(int id);
        /// <summary>
        /// 修改客户端的秘钥
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResultResponseDto> ModifySecret(ClientModifySecretRequestDto dto);


    }
}
