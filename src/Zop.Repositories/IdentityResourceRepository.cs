using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zop.Application.DataStore;
using Zop.Domain.Entities;
using Zop.Repositories.ChangeDetector;
using Zop.Repositories.Configuration;

namespace Zop.Repositories
{
    public class IdentityResourceRepository : EFRepository<IdentityResource, int>, IIdentityResourceDataStore
    {
        public IdentityResourceRepository(RepositoryDbContext dbContext, ILogger<ApiResourceRepository> logger, IChangeDetector changeDetector)
            : base(dbContext, logger, changeDetector)
        {

        }
        public Task<IList<IdentityResource>> GetAllAsync()
        {
            IList<IdentityResource> identityResources =
                this.dbContext.IdentityResources.ToList();
            return Task.FromResult(identityResources);
        }
        public override Task<IdentityResource> GetAsync(int id)
        {
            var identityResource = this.dbContext.IdentityResources.Where(f => f.Id == id).FirstOrDefault();
            return Task.FromResult(identityResource);
        }
        public Task<int> GetIdAsync(string name)
        {
            var id = this.dbContext.IdentityResources.Where(f => f.Name == name).Select(f => f.Id).FirstOrDefault();
            return Task.FromResult(id);
        }
        public Task<IList<IdentityResource>> GetListAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();
            var query = this.dbContext.IdentityResources.Where(f => scopes.Contains(f.Name));
            IList<IdentityResource> identityResources = query.ToList();
            return Task.FromResult(identityResources);
        }
    }
}
