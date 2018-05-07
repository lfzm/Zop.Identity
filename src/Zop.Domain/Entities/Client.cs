using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Zop.Domain.Values;
using Zop.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 客户端
    /// </summary>
    [Serializable]
    public class Client : AggregateConcurrencySafe<string>
    {
        #region 构造函数
        public Client() { }
        public Client(string clientId, string merchantId)
        {
            if (clientId.IsNull())
                throw new ArgumentNullException("客户端Id不能为空");
            if (merchantId.IsNull())
                throw new ArgumentNullException("商户ID不能为空");

            this.MerchantId = merchantId;
            this.SetId(clientId);
        }

        [JsonConstructor]
        private Client(string Id, string merchantId, IList<ClientSecret> Secrets, string AllowedGrantTypes, string RedirectUris, string AllowedScopes,
            IList<ClientProperty> Properties, string PostLogoutRedirectUris, string IdentityProviderRestrictions, string AllowedCorsOrigins,
            IList<ClientClaim> Claims, DateTime CreateTime)
        {
            this.Id = Id;
            this.Secrets = Secrets;
            this.AllowedGrantTypes = AllowedGrantTypes;
            this.RedirectUris = RedirectUris;
            this.AllowedScopes = AllowedScopes;
            this.Properties = Properties;
            this.PostLogoutRedirectUris = PostLogoutRedirectUris;
            this.IdentityProviderRestrictions = IdentityProviderRestrictions;
            this.AllowedCorsOrigins = AllowedCorsOrigins;
            this.Claims = Claims;
            this.CreateTime = CreateTime;
            this.MerchantId = merchantId;
        }
        #endregion

        #region 属性   
        #region Basics
        /// <summary>
        /// 客户端ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        public override string Id
        {
            get => base.Id;
            protected set => base.Id = value;
        }
        /// <summary>
        /// 商户ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string MerchantId { get; private set; }
        /// <summary>
        /// 是否启用(defaults to <c>true</c>)
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 密钥
        /// </summary>
        public IList<ClientSecret> Secrets { get; private set; } = new List<ClientSecret>();
        /// <summary>
        /// 是否需要密钥来从令牌端点请求令牌（默认为true）
        /// </summary>
        public bool RequireClientSecret { get; set; } = true;
        /// <summary>
        /// 客户端允许使用的授予类型。使用GrantTypes该类来进行常见组合
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string AllowedGrantTypes { get; private set; }
        /// <summary>
        /// 指定使用基于授权代码的授权类型的客户端是否必须发送证明密钥 (defaults to <c>false</c>).
        /// </summary>
        public bool RequirePkce { get; set; } = false;
        /// <summary>
        /// 使用PKCE的客户端是否可以使用纯文本代码（不推荐 - 并且默认为false）
        /// </summary>
        public bool AllowPlainTextPkce { get; set; } = false;
        /// <summary>
        /// 允许返回令牌或授权码的URI
        /// </summary>
        [MaxLength(2000)]
        public string RedirectUris { get; private set; }
        /// <summary>
        /// 默认登录地址
        /// </summary>
        [MaxLength(300)]
        public string LoginUri { get;  set; }
        /// <summary>
        /// 客户端访问范围资源
        /// </summary>
        [MaxLength(2000)]
        public string AllowedScopes { get; private set; }
        /// <summary>
        /// 客户端是否可以请求刷新离线令牌（请求offline_access范围）
        /// </summary>
        public bool AllowOfflineAccess { get; set; } = false;
        /// <summary>
        /// 是否允许浏览器传输访问令牌
        /// </summary>
        public bool AllowAccessTokensViaBrowser { get; set; } = false;
        /// <summary>
        ///字典根据需要保存任何自定义客户端特定的值。
        /// </summary>
        public IList<ClientProperty> Properties { get; private set; } = new List<ClientProperty>();
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(20000)]
        public string Description { get; set; }
        #endregion

        #region Consent Screen
        /// <summary>
        /// 客户名称
        /// </summary>
        [MaxLength(200)]
        public string ClientName { get; set; }
        /// <summary>
        /// 有关客户信息的URI
        /// </summary>
        [MaxLength(2000)]
        public string ClientUri { get; set; }
        /// <summary>
        /// Logo Uri
        /// </summary>
        [MaxLength(2000)]
        public string LogoUri { get; set; }

        /// <summary>
        /// 是否需要同意屏幕（默认为true）
        /// </summary>
        public bool RequireConsent { get; set; } = true;
        /// <summary>
        /// 用户是否可以选择存储同意决定（默认为true）
        /// </summary>
        public bool AllowRememberConsent { get; set; } = true;
        /// <summary>
        /// 用户同意的生命周期。默认为空（不过期）(秒)。
        /// </summary>
        public int? ConsentLifetime { get; set; } = null;
        #endregion

        #region 认证/注销
        /// <summary>
        /// 允许注销后重定向到URI
        /// </summary>
        [MaxLength(10000)]
        public string PostLogoutRedirectUris { get; private set; }
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
        /// 指定此客户端是否可以使用本地帐户，或仅使用外部IdP。默认为true。
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;
        /// <summary>
        /// 客户端可以用哪些外部IdP（如果列表为空，则允许所有IdP）。默认为空。
        /// </summary>
        [MaxLength(2000)]
        public string IdentityProviderRestrictions { get; private set; }
        /// <summary>
        /// 认证协议类型
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProtocolType { get; set; } = ProtocolTypes.OpenIdConnect;
        #endregion

        #region Token
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
        /// 如果指定将由默认的CORS策略服务实现（In-Memory和EF）为JavaScript客户端构建CORS策略。
        /// </summary>
        [MaxLength(2000)]
        public string AllowedCorsOrigins { get; private set; }
        /// <summary>
        /// 允许客户端的声明（将包含在访问令牌中）。
        /// </summary>
        public IList<ClientClaim> Claims { get; private set; } = new List<ClientClaim>();
        /// <summary>
        ///为true则客户声明将针对每个流程发送。为false则仅用于客户端凭据流（默认为false）
        /// </summary>
        public bool AlwaysSendClientClaims { get; set; } = false;
        /// <summary>
        /// 在请求id令牌和访问令牌时，用户声明应始终添加到id令牌中，而不是要求客户端使用userinfo端点。默认为false。
        /// </summary>
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;
        /// <summary>
        /// 为true前缀客户端声明类型将以前缀。默认为client_。其目的是确保它们不会意外与用户声明相冲突。
        /// </summary>
        [MaxLength(200)]
        public string ClientClaimsPrefix { get; set; } = "client_";
        /// <summary>
        /// 用于此客户的用户的pair-wise subjectId生成中的盐值
        /// </summary>
        [MaxLength(200)]
        public string PairWiseSubjectSalt { get; set; }

        #endregion
        #endregion

        /// <summary>
        /// 获取客户端允许使用的授予类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllowedGrantTypes()
        {
            return this.AllowedGrantTypes.GetList();
        }
        /// <summary>
        /// 设置客户端允许使用的授予类型
        /// </summary>
        /// <param name="callback"></param>
        public void SetAllowedGrantTypes(Func<string, string> callback)
        {
            this.AllowedGrantTypes = callback?.Invoke(this.AllowedGrantTypes);
        }
        /// <summary>
        /// 获取允许返回令牌或授权码的URI
        /// </summary>
        /// <returns></returns>
        public List<string> GetRedirectUris()
        {
            return this.RedirectUris.GetList();
        }
        /// <summary>
        /// 设置允许返回令牌或授权码的URI
        /// </summary>
        /// <param name="callback"></param>
        public void SetRedirectUris(Func<string, string> callback)
        {
            this.RedirectUris = callback?.Invoke(this.RedirectUris);
        }
        /// <summary>
        /// 获取客户端访问范围资源
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllowedScopes()
        {
            return this.AllowedScopes.GetList();
        }
        /// <summary>
        /// 设置客户端访问范围资源
        /// </summary>
        /// <param name="callback"></param>
        public void SetAllowedScopes(Func<string, string> callback)
        {
            this.AllowedScopes = callback?.Invoke(this.AllowedScopes);
        }
        /// <summary>
        /// 获取允许注销后重定向到URI
        /// </summary>
        /// <returns></returns>
        public List<string> GetPostLogoutRedirectUris()
        {
            return this.PostLogoutRedirectUris.GetList();
        }
        /// <summary>
        /// 设置允许注销后重定向到URI
        /// </summary>
        /// <param name="callback"></param>
        public void SetPostLogoutRedirectUris(Func<string, string> callback)
        {
            this.PostLogoutRedirectUris = callback?.Invoke(this.PostLogoutRedirectUris);
        }
        /// <summary>
        /// 获取客户端可以用哪些外部IdP
        /// </summary>
        /// <returns></returns>
        public List<string> GetIdentityProviderRestrictions()
        {
            return this.IdentityProviderRestrictions.GetList();
        }
        /// <summary>
        /// 设置客户端可以用哪些外部IdP
        /// </summary>
        /// <param name="callback"></param>
        public void SetIdentityProviderRestrictions(Func<string, string> callback)
        {
            this.IdentityProviderRestrictions = callback?.Invoke(this.IdentityProviderRestrictions);
        }
        /// <summary>
        /// 获取允许CORS策略
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllowedCorsOrigins()
        {
            return this.AllowedCorsOrigins.GetList();
        }
        /// <summary>
        /// 设置客户端可以用哪些外部IdP
        /// </summary>
        /// <param name="callback"></param>
        public void SetAllowedCorsOrigins(Func<string, string> callback)
        {
            this.AllowedCorsOrigins = callback?.Invoke(this.AllowedCorsOrigins);
        }

    }
}
