using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.AccessTokenValidation;
using System.Security.Cryptography;
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
            services.AddOrleansClient(this.Configuration.GetSection("OrleansClient"));
            //配置IdentityServer
            services.AddIdentityServer(opt =>
            {
                opt.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(30);
                opt.Caching.ResourceStoreExpiration = TimeSpan.FromHours(2);
                opt.Caching.CorsExpiration = TimeSpan.FromHours(1);
            })
            .AddConfigurationStoreCache()
            .AddSigningCredential(options.SigningCredentialRsa);
            //.AddDeveloperSigningCredential();//使用默认签名证书

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //自动跳转到https
                var options = new RewriteOptions().AddRedirectToHttpsPermanent();
                app.UseRewriter(options);
                app.UseExceptionHandler();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseSession();
            app.UseMvc();
        }
    }
}
