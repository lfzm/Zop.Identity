using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Hosting;
using Orleans;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Orleans.Configuration;

namespace Zop.Identity.Test.Configurator
{
    public class SiloBuilderConfigurator : ISiloBuilderConfigurator
    {
        public void Configure(ISiloHostBuilder hostBuilder)
        {
            hostBuilder.UseLocalhostClustering(siloPort: 1000, gatewayPort: 1001);
           
            hostBuilder.ConfigureServices(ConfigureServices);

        }


        public static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            const string connectionString = @"Database=zop_ids;Data Source=120.78.175.212;User Id=root;Password=zwcsroot;pooling=false;";
            services.AddAutoMapper(c=>
            {
                c.RegisterAllMappings(typeof(Zop.MigrationsDbContext).Assembly);
            });
            services.AddIdentityApplication();
            services.AddRepositoryStorage(rb =>
            {
                rb.AddIdentityRepository(options =>
                {
                    options.DbContextOptions = dbBuilder => dbBuilder.UseMySql(connectionString);
                });
            });
        }
    }
}
