using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Repositories.ChangeDetector;
using Zop.Repositories.Configuration;

namespace Zop.Repositories
{
    /// <summary>
    /// Entity Framework Core 仓储基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class EFRepository<TEntity, TPrimaryKey> : Repository<TEntity, TPrimaryKey>, IDisposable where TEntity : class, IEntity
    {
        public readonly RepositoryDbContext dbContext;
        private readonly ILogger Logger;
        public EFRepository(RepositoryDbContext dbContext, ILogger logger) : this(dbContext, logger, null)
        {

        }
        public EFRepository(RepositoryDbContext dbContext, ILogger logger,IChangeDetector changeDetector )
            :base(changeDetector)
        {
            this.Logger= logger;
            this.dbContext = dbContext;
        }
        public override Task DeleteAsync(TEntity entity)
        {
            this.dbContext.Delete(entity);
            int count = this.dbContext.SaveChanges();
            return Task.FromResult(count);
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            this.dbContext.Add(entity);
            this.dbContext.SaveChanges();
            return Task.FromResult(entity);
        }

        public async override Task<TEntity> UpdateAsync(TEntity entity)
        {
            IChangeManager changeManager = await this.GetChangeManagerAsync(entity);
            this.dbContext.Update(changeManager, entity);
            this.dbContext.SaveChanges();
            return entity;
        }

    }
}
