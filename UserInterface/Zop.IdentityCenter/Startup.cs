using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using Zop.Extensions.OrleansClient.Configuration;
using Zop.IdentityCenter.Configuration;

namespace Zop.IdentityCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityCenterOptions options = Configuration.GetSection("IdentityCenter").Get<IdentityCenterOptions>();

            services.AddAutoMapper();
            services.AddIdentityCenter(Configuration.GetSection("IdentityCenter"));
            //配置Orleans客户端
            services.AddOrleansClient(build =>
            {
                build.AddClient(Configuration.GetSection("OrleansClient:Identity"),b=>
                {
                    var c = Configuration.GetSection("OrleansClient:Identity").Get<OrleansClientOptions>();
                    b.UseConsulClustering((ConsulClusteringClientOptions opt) =>
                    {
                        opt.Address = new Uri( c.ConsulAddress);
                    });
                });
            });
            //配置IdentityServer
            services.AddIdentityServer(opt =>
            {
                opt.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(30);
                opt.Caching.ResourceStoreExpiration = TimeSpan.FromHours(2);
                opt.Caching.CorsExpiration = TimeSpan.FromHours(1);
            })
            .AddIdentityServiceStore()
            .AddConfigurationStoreCache()
            .AddSigningCredential(options.SigningCredentialRsa);
            services.AddSession();
            services.AddMvc();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //自动跳转到https
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default",
                   template: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
