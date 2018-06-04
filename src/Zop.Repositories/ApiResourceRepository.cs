using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class ApiResourceRepository : EFRepository<ApiResource, int>, IApiResourceDataStore
    {
        public ApiResourceRepository(RepositoryDbContext dbContext, ILogger<ApiResourceRepository> logger, IChangeDetector changeDetector)
            : base(dbContext, logger, changeDetector)
        {

        }


        public Task<IList<ApiResource>> GetAllAsync()
        {
            IList<ApiResource> apiResources =
                this.dbContext.ApiResources
                .Include(x => x.Scopes)
                .Include(x => x.Secrets)
                .ToList();
            return Task.FromResult(apiResources);
        }

        public override Task<ApiResource> GetAsync(int id)
        {
            if(id<=0)  return Task.FromResult<ApiResource>(null);

            var apiResource = this.dbContext.ApiResources
                .Include(x => x.Scopes)
                .Include(x => x.Secrets)
                .FirstOrDefault(x => x.Id == id);
            return Task.FromResult(apiResource);
        }

        public Task<int> GetIdAsync(string name)
        {
            int id = this.dbContext.ApiResources.Where(f => f.Name == name).Select(f => f.Id).FirstOrDefault();
            return Task.FromResult(id);
        }

        public Task<IList<ApiResource>> GetListAsync(IEnumerable<string> scopeNames)
        {
            var scopes = scopeNames.ToArray();
            var query = this.dbContext.ApiResources.Where(f => scopes.Contains(f.Name));
            IList<ApiResource> apiResources = query
                .Include(x => x.Scopes)
                .Include(x => x.Secrets)
                .ToList();
            return Task.FromResult(apiResources);
        }

        public Task<int> GetScopeIdAsync(string name)
        {
            var id = this.dbContext.ApiScopes.Where(f => f.Name == name).Select(f => f.Id).FirstOrDefault();
            return Task.FromResult(id);
        }
    }
}
