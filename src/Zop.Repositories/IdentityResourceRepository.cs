using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Repositories.Configuration;

namespace Zop.Repositories
{
    public class IdentityResourceRepository : EFRepository<IdentityResource, int>, IIdentityResourceRepositories
    {
        public IdentityResourceRepository(RepositoryDbContext dbContext, ILogger<ApiResourceRepository> logger, IServiceProvider serviceProvider)
            : base(dbContext, logger, serviceProvider)
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
            if (id <= 0) return Task.FromResult<IdentityResource>(null);

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
