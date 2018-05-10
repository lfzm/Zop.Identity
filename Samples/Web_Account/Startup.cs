using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace Web_Account
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
            //配置认证服务
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                opt.Cookie.Name = "123123123";
            })
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.Authority = "http://localhost:5000/";
                opt.RequireHttpsMetadata = false;

                opt.ClientSecret = "secret";
                opt.ClientId = "164965530879528960";
                opt.ResponseType = "code id_token";//使用Hybrid认证
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.SaveTokens = true;

                opt.Scope.Clear();
                opt.Scope.Add("IDC_API");
                opt.Scope.Add("openid");
                opt.Scope.Add("profile");
                opt.Scope.Add("phone");
                opt.Scope.Add("offline_access");
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
