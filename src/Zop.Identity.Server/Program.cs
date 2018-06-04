using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using System.Net;
using Orleans.Configuration;
using Orleans.Runtime;
using Zop.Application.Services;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Orleans.Authentication.IdentityServer4;
using Orleans.Authentication;
using System.Net.Sockets;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Zop.Repositories.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Zop.Identity.Server
{
    public class Program
    {
        static readonly ManualResetEvent _siloStopped = new ManualResetEvent(false);
        static ISiloHost silo;
        static bool siloStopping = false;
        static readonly object syncLock = new object();
        public static void Main(string[] args)
        {
            try
            {
                SetupApplicationShutdown();

                silo = CreateSilo();
                silo.StartAsync().Wait();
                Console.WriteLine(DateTime.Now + "  Zop.Identity.Server Start...");
                /// Wait for the silo to completely shutdown before exiting. 
                _siloStopped.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                return;
            }
        }
        static void SetupApplicationShutdown()
        {
            /// Capture the user pressing Ctrl+C
            Console.CancelKeyPress += (s, a) =>
            {
                /// Prevent the application from crashing ungracefully.
                a.Cancel = true;
                /// Don't allow the following code to repeat if the user presses Ctrl+C repeatedly.
                lock (syncLock)
                {
                    if (!siloStopping)
                    {
                        siloStopping = true;
                        Task.Run(StopSilo).Ignore();
                    }
                }
                /// Event handler execution exits immediately, leaving the silo shutdown running on a background thread,
                /// but the app doesn't crash because a.Cancel has been set = true
            };
        }
        static ISiloHost CreateSilo()
        {
            var builder = new SiloHostBuilder()
                .UseDashboard(opt =>
                {
                    opt.Port = 1010;
                })
                .UseConsulClustering((OptionsBuilder<ConsulClusteringSiloOptions> opt) =>
                {
                    opt.Configure<IConfiguration>((options, config) =>
                    {
                        var c = config.GetSection("IdentityOptions").Get<ConsulClusteringSiloOptions>();
                        options.Address = c.Address;
                    });
                })
              
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Zop.Identity";
                    options.ServiceId = "Zop.Identity";
                })
                //.ConfigureEndpoints(Dns.GetHostName(),siloPort:11111, gatewayPort:10000, listenOnAnyHostAddress:true)
                .ConfigureEndpoints(IPAddress.Parse("120.78.175.212"), siloPort: 11111, gatewayPort: 10000, listenOnAnyHostAddress: true)
                .ConfigureAppConfiguration((HostBuilderContext context, IConfigurationBuilder config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .ConfigureServices((HostBuilderContext context, IServiceCollection services)=>
                {
                    services.AddAutoMapper();
                    services.AddIdentityApplication();
                    services.AddRepositoryStorage(rb =>
                    {
                        var ro = context.Configuration.GetSection("IdentityRepositories").Get<RepositoryOptions>();
                        rb.AddIdentityRepository(options =>
                        {
                            options.DbContextOptions = dbBuilder => dbBuilder.UseMySql(ro.ConnectionString);
                        });
                    });
                })
                .ConfigureLogging((HostBuilderContext context, ILoggingBuilder logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddFile(context.Configuration.GetSection("Logging:File"));
                })
                .Configure<ProcessExitHandlingOptions>(options => options.FastKillOnProcessExit = false)
                .AddAuthentication((context, build) =>
                {
                    build.AddIdentityServerAuthentication(opt =>
                    {
                        var config = context.Configuration.GetSection("IdentityOptions").Get<IdentityServerAuthenticationOptions>();
                        opt.RequireHttpsMetadata = config.Authority.Contains("https/");
                        opt.Authority = config.Authority;
                        opt.ApiName = "IDS_OL_API";
                    });

                }, IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddAuthorizationFilter()
                .AddExceptionsFilter();

            var host = builder.Build();
            return host;
        }

        static async Task StopSilo()
        {
            await silo.StopAsync();
            _siloStopped.Set();
        }
    }
}
