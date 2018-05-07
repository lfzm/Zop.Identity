using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using System.Net;
using Orleans.Runtime;

namespace Zop.Identity.Test.Configurator
{
    public class ClientBuilderConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
        {
            clientBuilder
                .UseLocalhostClustering(gatewayPort: 1001);
        }
    }
}
