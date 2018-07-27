using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Repositories.Configuration;

namespace Zop.Repositories
{
    public class PersistedGrantRepository : EFRepository<PersistedGrant, string>, IPersistedGrantRepositories
    {
        public PersistedGrantRepository(RepositoryDbContext dbContext, ILogger<ApiResourceRepository> logger, IServiceProvider serviceProvider)
            : base(dbContext, logger, serviceProvider)
        {

        }
        public override Task<PersistedGrant> GetAsync(string id)
        {
            if (id.IsNull())
                return Task.FromResult<PersistedGrant>(null);
            var entity = this.dbContext.PersistedGrants.Where(f => f.Id == id).FirstOrDefault();
            return Task.FromResult(entity);
        }

        public Task<IList<string>> GetKeysAsync(string subjectId)
        {
            IList<string> ids = this.dbContext.PersistedGrants.Where(f => f.SubjectId == subjectId).Select(f => f.Id).ToList();
            return Task.FromResult(ids);
        }

        public Task<IList<string>> GetKeysAsync(string subjectId, string clientId)
        {
            IList<string> ids = this.dbContext.PersistedGrants.Where(f => f.SubjectId == subjectId && f.ClientId == clientId).Select(f => f.Id).ToList();
            return Task.FromResult(ids);
        }

        public Task<IList<string>> GetKeysAsync(string subjectId, string clientId, string type)
        {
            IList<string> ids = this.dbContext.PersistedGrants.Where(f => f.SubjectId == subjectId && f.ClientId == clientId && f.Type == type).Select(f => f.Id).ToList();
            return Task.FromResult(ids);
        }

        public Task<IList<PersistedGrant>> GetListAsync(string subjectId)
        {
            IList<PersistedGrant> entitys = this.dbContext.PersistedGrants.Where(f => f.SubjectId == subjectId).ToList();
            return Task.FromResult(entitys);
        }
    }
}
