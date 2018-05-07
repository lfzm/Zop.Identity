using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Zop.DTO;
using Zop.Identity;

namespace Zop.Identity.DTO.Request
{
    /// <summary>
    /// 修改客户端Token配置Dto
    /// </summary>
   public class ClientSetTokenRequestDto:RequestDto
    {
        /// <summary>
        /// 客户端是否可以请求刷新离线令牌（请求offline_access范围）
        /// </summary>
        public bool AllowOfflineAccess { get; set; } = false;
        /// <summary>
        /// 是否允许浏览器传输访问令牌
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; } = false;
        /// <summary>
        /// 身份标识生命周期（秒)( 默认为300秒/ 5分钟）
        /// </summary>
        public int IdentityTokenLifetime { get; set; } = 300;
        /// <summary>
        /// 访问令牌的生存期(秒)（默认为3600秒/ 1小时）
        /// </summary>
        public int AccessTokenLifetime { get; set; } = 3600;
        /// <summary>
        /// 授权码的生存时间(秒)（默认为300秒/ 5分钟）
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; } = 300;
        /// <summary>
        /// 刷新令牌的最大生存期 (秒)(默认为2592000秒/ 30天)
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        /// <summary>
        /// 滑动刷新令牌的生命周期 (秒)(默认为1296000秒/ 15天)
        /// </summary>
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        /// <summary>
        /// ReUse: 刷新令牌时，刷新令牌句柄将保持不变
        /// OneTime: 当刷新令牌时刷新令牌句柄将被更新。这是默认设置。
        /// </summary>
        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;
        /// <summary>
        /// Absolute: 刷新令牌将在固定时间点过期（由AbsoluteRefreshTokenLifetime指定）
        /// Sliding:  刷新令牌时刷新令牌的生命周期将会更新（按SlidingRefreshTokenLifetime中指定的生命周期）。生命周期不会超过AbsoluteRefreshTokenLifetime。
        /// </summary>        
        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;
        /// <summary>
        /// 是否应该在刷新令牌请求时更新访问令牌
        /// </summary>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; } = false;
        /// <summary>
        /// 访问令牌是参考令牌还是自包含的JWT令牌（默认为Jwt）
        /// </summary>
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
        /// <summary>
        /// JWT访问令牌是否应具有嵌入的唯一ID
        /// </summary>
        public bool IncludeJwtId { get; set; } = false;
        /// <summary>
        ///为true则客户声明将针对每个流程发送。为false则仅用于客户端凭据流（默认为false）
        /// </summary>
        public bool AlwaysSendClientClaims { get; set; } = false;
        /// <summary>
        /// 在请求id令牌和访问令牌时，用户声明应始终添加到id令牌中，而不是要求客户端使用userinfo端点。默认为false。
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

    }
}
