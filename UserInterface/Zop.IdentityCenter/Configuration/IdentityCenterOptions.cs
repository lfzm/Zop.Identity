using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Zop.Toolkit.Security;

namespace Zop.IdentityCenter.Configuration
{
    /// <summary>
    /// 认证中心的配置
    /// </summary>
    public class IdentityCenterOptions
    {
        /// <summary>
        /// 是否允许记住登录
        /// </summary>
        public bool AllowRememberLogin { get; set; } = true;
        /// <summary>
        /// 记住登录生命周期
        /// </summary>
        public TimeSpan RememberLoginLifetime { get; set; } = TimeSpan.FromDays(30);
        /// <summary>
        /// 默认登录地址
        /// </summary>
        public string DefaultLoginUrl { get; set; }
        /// <summary>
        /// 授权地址
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// Api秘钥
        /// </summary>
        public string ApiSecret { get; set; }
        /// <summary>
        /// Api名称
        /// </summary>
        public string ApiName { get; set; } = "IDC_API";
        /// <summary>
        /// 签名证书私钥 2048 RSA PKCS1
        /// </summary>
        public string SigningCredential { get; set; }
        /// <summary>
        /// RSA签名证书
        /// </summary>
        public RsaSecurityKey SigningCredentialRsa
        {
            get
            {
                RSAParameters param = Zop.Toolkit.Security.RSA.DecodePkcsPrivateKey(SigningCredential);
                return new RsaSecurityKey(param);
            }
        }
    }
}
