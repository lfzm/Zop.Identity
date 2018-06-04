using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Zop.IdentityCenter.Configuration;
using Orleans.Hosting;
using Orleans.Configuration;
using Zop.OrleansClient.Configuration;

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
                build.AddAuthentication(Configuration.GetSection("OrleansClient"));
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

            services.AddAuthentication().AddIdentityServerAuthentication("idc", opt =>
            {
                opt.RequireHttpsMetadata = options.Authority.Contains("https/");
                opt.Authority = options.Authority;
                opt.ApiSecret = options.ApiSecret;
                opt.ApiName = options.ApiName;
            });

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
                var options = new RewriteOptions().AddRedirectToHttpsPermanent();
                //app.UseRewriter(options);
                app.UseExceptionHandler();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseAuthentication();
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
