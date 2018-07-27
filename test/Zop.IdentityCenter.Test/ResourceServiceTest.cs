using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using Xunit;
using Zop.IdentityCenter.Application;

namespace Zop.IdentityCenter.Test
{
    public class ResourceServiceTest
    {
        [Fact]
        public void FindIdentityResourcesByScopeAsync()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOrleansClient(build =>
            {
                build.AddClient(opt =>
                {
                    opt.ServiceId = "Zop.Identity";
                },
                b =>
                {
                    b.UseConsulClustering((ConsulClusteringClientOptions opt) =>
                    {
                        opt.Address = new Uri("http://120.78.175.212:8500");
                    });
                });
            });
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddLogging();
            services.AddAutoMapper(c =>
            {
                c.RegisterAllMappings(typeof(ResourceService).Assembly);
            });
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IResourceService>();

           var r= service.FindIdentityResourcesByScopeAsync(new List<string>()
            {
                "openid","profile","username","COTC_API","UC_API","offline_access"
            }).Result;
        }
    }
}
