using Microsoft.Extensions.Configuration;
using System;
using Zop.Domain.Entities;
using Zop.Repositories;
using Zop.Repositories.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityRepositoryBuilderExtensions
    {
        /// <summary>
        /// 添加仓储配置
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="server"><see cref="RepositoryBuilder"/></param>
        /// <param name="storeOptionsAction">仓储配置设置函数</param>
        /// <returns></returns>
        public static RepositoryBuilder AddIdentityRepository(this RepositoryBuilder builder, IConfiguration configuration)
        {
            builder.Service.AddOptions().Configure<RepositoryOptions>(configuration);
            builder.AddIdentityRepository(configuration.Get<RepositoryOptions>());
            return builder;
        }
        /// <summary>
        /// 添加仓储配置
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="server"><see cref="RepositoryBuilder"/></param>
        /// <param name="storeOptionsAction">仓储配置设置函数</param>
        /// <returns></returns>
        public static RepositoryBuilder AddIdentityRepository(this RepositoryBuilder builder, Action<RepositoryOptions> action)
        {
            RepositoryOptions options = new RepositoryOptions();
            action?.Invoke(options);

            builder.Service.AddOptions().Configure(action);
            builder.AddIdentityRepository(options);
            return builder;
        }
        /// <summary>
        /// 添加仓储配置
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="server"><see cref="RepositoryBuilder"/></param>
        /// <param name="storeOptionsAction">仓储配置设置函数</param>
        /// <returns></returns>
        public static RepositoryBuilder AddIdentityRepository(this RepositoryBuilder builder, RepositoryOptions options)
        {
            builder.Service.AddScoped<RepositoryDbContext>();
            //builder.Service.AddDbContext<RepositoryDbContext>(options.DbContextOptions);

            //添加仓储服务
            builder.AddRepository<ClientRepository, Client>();
            builder.AddRepository<ApiResourceRepository, ApiResource>();
            builder.AddRepository<IdentityResourceRepository, IdentityResource>();
            builder.AddRepository<PersistedGrantRepository, PersistedGrant>();
            builder.AddRepository<IdentityTokenRepository, IdentityToken>();

            //添加数据商店服务
            builder.Service.AddTransient<IApiResourceRepositories, ApiResourceRepository>();
            builder.Service.AddTransient<IClientRepositories, ClientRepository>();
            builder.Service.AddTransient<IIdentityResourceRepositories, IdentityResourceRepository>();
            builder.Service.AddTransient<IPersistedGrantRepositories, PersistedGrantRepository>();

            return builder;
        }
    }
}
