using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Application.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityApplicationServiceCollectionExtensions
    {
        /// <summary>
        /// 添加授权应用服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="storeOptionsAction">服务配置</param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityApplication(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions().Configure<Opations>(configuration);
            service.AddIdentityApplication();
            return service;
        }
        /// <summary>
        /// 添加授权应用服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="action">服务配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityApplication(this IServiceCollection service, Action<Opations> action)
        {
            service.AddOptions().Configure(action);
            service.AddIdentityApplication();
            return service;
        }

        /// <summary>
        /// 添加授权应用服务
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
        {
            services.AddIDGenerator();
            return services;
        }

    }
}
