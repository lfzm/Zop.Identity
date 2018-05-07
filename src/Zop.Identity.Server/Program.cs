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
                Console.WriteLine(DateTime.Now + "  Zop Payment Server Start...");
                /// Wait for the silo to completely shutdown before exiting. 
                _siloStopped.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            var siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;

            var builder = new SiloHostBuilder()
                .UseEnvironment("Development")
                .UseLocalhostClustering(siloPort, gatewayPort)
                .ConfigureAppConfiguration(Startup.ConfigureAppConfiguration)
                .ConfigureServices(Startup.ConfigureServices)
                .ConfigureLogging((HostBuilderContext context, ILoggingBuilder logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddFile(context.Configuration.GetSection("Logging:File"));
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = siloAddress;
                    options.SiloPort = siloPort;
                    options.GatewayPort = gatewayPort;
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Zop.Identity";
                    options.ServiceId = "Zop.Identity";
                })
                .Configure<ProcessExitHandlingOptions>(options => options.FastKillOnProcessExit = false)
                .AddAuthentication((context, build) =>
                {
                    build.AddIdentityServerAuthentication(opt =>
                    {
                        var config = context.Configuration.GetSection("ApiAuth").Get<IdentityServerAuthenticationOptions>();
                        opt.RequireHttpsMetadata = config.Authority.Contains("https/");
                        opt.Authority = config.Authority;
                        opt.ApiName = config.ApiName;
                        opt.ApiSecret = config.ApiSecret;
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
