using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.IdentityCenter.Application;
using Zop.IdentityCenter.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityCenterServiceCollectionExtensions
    {
        /// <summary>
        /// 添加IdentityServer仓储服务
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder">IdentityServerBuilder</param>
        /// <param name="storeOptionsAction">仓储配置设置函数</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddStore(this IIdentityServerBuilder builder)
        {
            //注入IdentityServer配置组件
            builder.Services.AddTransient<IClientStore, ClientService>();
            builder.Services.AddTransient<IResourceStore, ResourceService>();
            builder.Services.AddTransient<ICorsPolicyService, ClientService>();
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantService>();

            return builder;
        }

        /// <summary>
        /// 添加仓储配置(高速缓存)
        /// </summary>
        /// <param name="builder">IdentityServerBuilder</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfigurationStoreCache(
                 this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // add the caching decorators
            builder.AddClientStoreCache<ClientService>();
            builder.AddResourceStoreCache<ResourceService>();
            builder.AddCorsPolicyCache<ClientService>();
            return builder;
        }

        /// <summary>
        /// 添加认证中心服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="storeOptionsAction">领域服务配置</param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityCenter(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions().Configure<IdentityCenterOptions>(configuration);
            service.AddIdentityCenter();
            return service;
        }
        /// <summary>
        /// 添加认证中心服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="action">领域服务配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityCenter(this IServiceCollection service, Action<IdentityCenterOptions> action)
        {
            service.AddOptions().Configure(action);
            service.AddIdentityCenter();
            return service;
        }

        public static IServiceCollection AddIdentityCenter(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            return services;
        }

    }
}
