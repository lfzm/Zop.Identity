using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Identity
{
    public static class GrantType
    {
        public const string Implicit = "implicit";
        public const string Hybrid = "hybrid";
        public const string AuthorizationCode = "authorization_code";
        public const string ClientCredentials = "client_credentials";
        public const string ResourceOwnerPassword = "password";
    }
}
