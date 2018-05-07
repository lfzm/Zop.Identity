using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.Configuration
{
    /// <summary>
    /// 仓储的表名
    /// </summary>
   public class TableConfiguration
    {
        /// <summary>
        /// Gets or sets the identity resource table configuration.
        /// </summary>
        /// <value>
        /// The identity resource.
        /// </value>
        public string IdentityResource { get; set; } = ("IdentityResources");
   
        /// <summary>
        /// Gets or sets the API resource table configuration.
        /// </summary>
        /// <value>
        /// The API resource.
        /// </value>
        public string ApiResource { get; set; } = ("ApiResources");
        /// <summary>
        /// Gets or sets the API secret table configuration.
        /// </summary>
        /// <value>
        /// The API secret.
        /// </value>
        public string  ApiSecret { get; set; } = ("ApiSecrets");
        /// <summary>
        /// Gets or sets the API scope table configuration.
        /// </summary>
        /// <value>
        /// The API scope.
        /// </value>
        public string  ApiScope { get; set; } = ("ApiScopes");
        /// <summary>
        /// Gets or sets the client table configuration.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public string  Client { get; set; } = ("Clients");

        /// <summary>
        /// Gets or sets the client secret table configuration.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string  ClientSecret { get; set; } = ("ClientSecrets");
        /// <summary>
        /// Gets or sets the client claim table configuration.
        /// </summary>
        /// <value>
        /// The client claim.
        /// </value>
        public string  ClientClaim { get; set; } = ("ClientClaims");
        /// <summary>
        /// Gets or sets the client property table configuration.
        /// </summary>
        /// <value>
        /// The client property.
        /// </value>
        public string  ClientProperty { get; set; } = ("ClientProperties");
        /// <summary>
        /// Gets or sets the PersistedGrant table configuration.
        /// </summary>
        /// <value>
        /// The client property.
        /// </value>
        public string  PersistedGrant { get; set; } = ("PersistedGrant");
        /// <summary>
        /// Gets or sets the IdentityToken table configuration.
        /// </summary>
        /// <value>
        /// The client property.
        /// </value>
        public string IdentityToken { get; set; } = ("IdentityToken");
    }
}
