using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Orleans.Authentication;

using Orleans.Hosting;
using Orleans;
using System.IO;
using Orleans.Configuration;
using Orleans.Authentication.IdentityServer4;
using Zop.Repositories.Configuration;
using System.Reflection;

namespace Zop.Identity.Server
{
    /// <summary>
    /// 服务器启动
    /// </summary>
    public static class Startup
    {
        public static void ConfigureAppConfiguration(HostBuilderContext builder, IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.HostingEnvironment.EnvironmentName}.json", optional: true);
        }
        public static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            const string connectionString = @"Database=zop_ids;Data Source=120.78.175.212;User Id=root;Password=zwcsroot;pooling=false;";
            services.AddAutoMapper();
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

