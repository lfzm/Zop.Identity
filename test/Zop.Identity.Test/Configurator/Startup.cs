using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Identity.Test.Configurator;

namespace Zop.Identity.Test
{
    public static class Startup
    {
        public static TestCluster CreateCluster()
        {
            var builder = new TestClusterBuilder(1);
            builder.Options.BaseSiloPort = 1000; // this works, while ISiloBuilderConfigurator does not
            builder.Options.BaseGatewayPort = 1001;
            builder.AddClientBuilderConfigurator<ClientBuilderConfigurator>();
            builder.AddSiloBuilderConfigurator<SiloBuilderConfigurator>();
            var cluster = builder.Build();
            cluster.DeployAsync().Wait();
            return cluster;
        }
    }
}
