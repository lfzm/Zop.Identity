using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;

namespace Zop.Repositories.Configuration
{
    /// <summary>
    /// 仓储数据上下文
    /// </summary>
    public class RepositoryDbContext : DbContext
    {
        protected readonly RepositoryOptions options;
        private readonly ILoggerFactory loggerFactory;
        public RepositoryDbContext(IOptions<RepositoryOptions> options, ILoggerFactory loggerFactory)
        {
            this.options = options?.Value;
            if (this.options == null)
                this.options = new RepositoryOptions();
            this.loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(this.loggerFactory);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);    //默认不进行查询跟踪
            this.options?.DbContextOptions?.Invoke(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(client =>
            {
                client.ToTable(options.TableConfig.Client);
                client.HasKey(f => f.Id);
                client.Property(f => f.Id).ValueGeneratedNever();
            });
            modelBuilder.Entity<ClientClaim>(clientClaim =>
            {
                clientClaim.ToTable(options.TableConfig.ClientClaim);
            });
            modelBuilder.Entity<ClientProperty>(clientProperty =>
            {
                clientProperty.ToTable(options.TableConfig.ClientProperty);
            });
            modelBuilder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable(options.TableConfig.ApiResource);
                apiResource.HasIndex(f => f.Name).IsUnique();
            });
            modelBuilder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable(options.TableConfig.ApiScope);
                apiScope.HasIndex(f => f.Name).IsUnique();
            });
            modelBuilder.Entity<Secret>(apiSecret =>
            {
                apiSecret.ToTable(options.TableConfig.Secret);
            
            });
            modelBuilder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable(options.TableConfig.IdentityResource);
                identityResource.HasIndex(f => f.Name).IsUnique();
            });
            modelBuilder.Entity<IdentityToken>(identityGrant =>
            {
                identityGrant.ToTable(options.TableConfig.IdentityToken);
            });
            modelBuilder.Entity<PersistedGrant>(persistedGrant =>
            {
                persistedGrant.ToTable(options.TableConfig.PersistedGrant);
            });
            base.OnModelCreating(modelBuilder);
        }

    
        /// <summary>
        /// 客户端
        /// </summary>
        public DbSet<Client> Clients { get; set; }
        /// <summary>
        /// 身份认证资源
        /// </summary>
        public DbSet<IdentityResource> IdentityResources { get; set; }
        /// <summary>
        /// API资源
        /// </summary>
        public DbSet<ApiResource> ApiResources { get; set; }
        /// <summary>
        /// API资源范围
        /// </summary>
        public DbSet<ApiScope> ApiScopes { get; set; }
        /// <summary>
        /// 发放的令牌
        /// </summary>
        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        /// <summary>
        /// 身份令牌
        /// </summary>
        public DbSet<IdentityToken> IdentityTokens { get; set; }

    }
}
