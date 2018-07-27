using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Zop.Repositories.Configuration;

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
                silo = CreateSilo(args);
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
        static ISiloHost CreateSilo(string[] args)
        {
            (int siloPort ,int gatewayPort, IPAddress advertisedIP) = GetEndpointsOption(args);
            Console.WriteLine(($"siloPort：{siloPort}，gatewayPort：{gatewayPort}，advertisedIP：{advertisedIP}"));

            //启动SiloHost
            var builder = new SiloHostBuilder()
                .UseDashboard(opt => opt.HostSelf = false)
                .UseConsulClustering((OptionsBuilder<ConsulClusteringSiloOptions> opt) =>
                {
                    opt.Configure<IConfiguration>((options, config) =>
                    {
                        var c = config.GetSection("IdentityOptions").Get<ConsulClusteringSiloOptions>();
                        options.Address = c.Address;
                    });
                })
                .ConfigureHostConfiguration((IConfigurationBuilder config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                            .AddCommandLine(args)
                            .AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true);
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Zop.Identity";
                    options.ServiceId = "Zop.Identity";
                })
                .ConfigureEndpoints(advertisedIP, siloPort: siloPort, gatewayPort: gatewayPort, listenOnAnyHostAddress: true)
                .ConfigureAppConfiguration((HostBuilderContext context, IConfigurationBuilder config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
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
                .AddExceptionsFilter();

            var host = builder.Build();
            return host;
        }

        static async Task StopSilo()
        {
            await silo.StopAsync();
            _siloStopped.Set();
        }

        static ValueTuple<int, int, IPAddress> GetEndpointsOption(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                              .AddCommandLine(args)
                              .AddJsonFile("hostsettings.json", optional: true, reloadOnChange: true)
                              .Build();
            IPAddress advertisedIP;
            int siloPort = 0;
            int gatewayPort = 0;
            if (!configuration["SiloPort"].IsNull())
                siloPort = int.Parse(configuration["SiloPort"]);
            else
                siloPort = ConfigUtilities.GetRandAvailablePort();
            if (!configuration["GatewayPort"].IsNull())
                gatewayPort = int.Parse(configuration["GatewayPort"]);
            else
                gatewayPort = ConfigUtilities.GetRandAvailablePort(ignorePort: siloPort);
            if (!configuration["AdvertisedIP"].IsNull())
                advertisedIP = IPAddress.Parse(configuration["AdvertisedIP"]);
            else
                advertisedIP = ConfigUtilities.ResolveIPAddress(Dns.GetHostName(), null, AddressFamily.InterNetwork).Result;

            return ValueTuple.Create<int, int, IPAddress>(siloPort, gatewayPort, advertisedIP);
        
        }
    }
}
