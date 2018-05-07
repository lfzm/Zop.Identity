using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Zop.Identity.DTO
{
    public class ClientDto
    {

        #region Basics
        /// <summary>
        /// 是否启用(defaults to <c>true</c>)
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public ICollection<SecretDto> ClientSecrets { get; set; }
        /// <summary>
        /// 是否需要密钥来从令牌端点请求令牌（默认为true）
        /// </summary>
        public bool RequireClientSecret { get; set; }
        /// <summary>
        /// 客户端允许使用的授予类型。使用GrantTypes该类来进行常见组合
        /// </summary>
        public ICollection<string> AllowedGrantTypes { get; set; }
        /// <summary>
        /// 指定使用基于授权代码的授权类型的客户端是否必须发送证明密钥 (defaults to <c>false</c>).
        /// </summary>
        public bool RequirePkce { get; set; }
        /// <summary>
        /// 使用PKCE的客户端是否可以使用纯文本代码（不推荐 - 并且默认为false）
        /// </summary>
        public bool AllowPlainTextPkce { get; set; }
        /// <summary>
        /// 允许返回令牌或授权码的URI
        /// </summary>
        public ICollection<string> RedirectUris { get; set; } 
        /// <summary>
        /// 客户端访问范围资源
        /// </summary>
        public ICollection<string> AllowedScopes { get; set; }
        /// <summary>
        /// 客户端是否可以请求刷新离线令牌（请求offline_access范围）
        /// </summary>
        public bool AllowOfflineAccess { get; set; }
        /// <summary>
        /// 是否允许浏览器传输访问令牌
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; }
        /// <summary>
        ///字典根据需要保存任何自定义客户端特定的值。
        /// </summary>
        public IDictionary<string, string> Properties { get; set; } 
        #endregion

        #region Consent Screen
        /// <summary>
        /// 客户名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 有关客户信息的URI
        /// </summary>
        public string ClientUri { get; set; }

        /// <summary>
        /// Logo Uri
        /// </summary>
        public string LogoUri { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// 是否需要同意屏幕（默认为true）
        /// </summary>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// 用户是否可以选择存储同意决定（默认为true）
        /// </summary>
        public bool AllowRememberConsent { get; set; }

        /// <summary>
        /// 用户同意的生命周期。默认为空（不过期）(秒)。
        /// </summary>
        public int? ConsentLifetime { get; set; }
        #endregion

        #region 认证/注销
        /// <summary>
        /// 允许的URI在注销后重定向到
        /// </summary>
        public ICollection<string> PostLogoutRedirectUris { get; set; } 
        /// <summary>
        /// 指定基于HTTP的前台通道注销的注销URI
        /// </summary>
        public string FrontChannelLogoutUri { get; set; }

        /// <summary>
        /// 指定是否应将用户的会话标识发送到FrontChannelLogoutUri。默认为true。
        /// </summary>
        public bool FrontChannelLogoutSessionRequired { get; set; } 

        /// <summary>
        /// 指定基于HTTP的反向通道注销的注销URI
        /// </summary>
        public string BackChannelLogoutUri { get; set; }

        /// <summary>
        /// 指定是否在请求中将用户的会话ID发送到BackChannelLogoutUri。默认为true。
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; }
        /// <summary>
        /// 指定此客户端是否可以使用本地帐户，或仅使用外部IdP。默认为true。
        /// </summary>
        public bool EnableLocalLogin { get; set; }
        /// <summary>
        /// 指定哪些外部IdP可以用于此客户端（如果列表为空，则允许所有IdP）。默认为空。
        /// </summary>
        public ICollection<string> IdentityProviderRestrictions { get; set; }
        #endregion

        #region Token
        /// <summary>
        /// 身份标识生命周期（秒)( 默认为300秒/ 5分钟）
        /// </summary>
        public int IdentityTokenLifetime { get; set; }
        /// <summary>
        /// 访问令牌的生存期(秒)（默认为3600秒/ 1小时）
        /// </summary>
        public int AccessTokenLifetime { get; set; }
        /// <summary>
        /// 授权码的生存时间(秒)（默认为300秒/ 5分钟）
        /// </summary>
        public int AuthorizationCodeLifetime { get; set; }
        /// <summary>
        /// 刷新令牌的最大生存期 (秒)(默认为2592000秒/ 30天)
        /// </summary>
        public int AbsoluteRefreshTokenLifetime { get; set; } 
        /// <summary>
        /// 滑动刷新令牌的生命周期 (秒)(默认为1296000秒/ 15天)
        /// </summary>
        public int SlidingRefreshTokenLifetime { get; set; }
        /// <summary>
        /// ReUse: 刷新令牌时，刷新令牌句柄将保持不变
        /// OneTime: 当刷新令牌时刷新令牌句柄将被更新。这是默认设置。
        /// </summary>
        public TokenUsage RefreshTokenUsage { get; set; }
        /// <summary>
        /// Absolute: 刷新令牌将在固定时间点过期（由AbsoluteRefreshTokenLifetime指定）
        /// Sliding:  刷新令牌时刷新令牌的生命周期将会更新（按SlidingRefreshTokenLifetime中指定的生命周期）。生命周期不会超过AbsoluteRefreshTokenLifetime。
        /// </summary>        
        public TokenExpiration RefreshTokenExpiration { get; set; }
        /// <summary>
        /// 是否应该在刷新令牌请求时更新访问令牌
        /// </summary>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        /// <summary>
        /// 访问令牌是参考令牌还是自包含的JWT令牌（默认为Jwt）
        /// </summary>
        public AccessTokenType AccessTokenType { get; set; }
        /// <summary>
        /// JWT访问令牌是否应具有嵌入的唯一ID
        /// </summary>
        public bool IncludeJwtId { get; set; } 
        /// <summary>
        /// 如果指定将由默认的CORS策略服务实现（In-Memory和EF）为JavaScript客户端构建CORS策略。
        /// </summary>
        public ICollection<string> AllowedCorsOrigins { get; set; }
        /// <summary>
        /// 允许客户端的设置声明（将包含在访问令牌中）。
        /// </summary>
        public ICollection<ClaimDto> Claims { get; set; } 

        /// <summary>
        ///为true则客户声明将针对每个流程发送。为false则仅用于客户端凭据流（默认为false）
        /// </summary>
        public bool AlwaysSendClientClaims { get; set; } 
        /// <summary>
        /// 在请求id令牌和访问令牌时，用户声明应始终添加到id令牌中，而不是要求客户端使用userinfo端点。默认为false。
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        /// <summary>
        /// 为true前缀客户端声明类型将以前缀。默认为client_。其目的是确保它们不会意外与用户声明相冲突。
        /// </summary>
        public string ClientClaimsPrefix { get; set; }
        /// <summary>
        /// 用于此客户的用户的pair-wise subjectId生成中的盐值
        /// </summary>
        public string PairWiseSubjectSalt { get; set; }
        #endregion
    }
}
