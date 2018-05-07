using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zop.Application.DataStore;
using Zop.Domain.Entities;
using Zop.Repositories.ChangeDetector;
using Zop.Repositories.Configuration;

namespace Zop.Repositories
{
    public class ClientRepository : EFRepository<Client, string>, IClientDataStore, IDisposable
    {

        public ClientRepository(RepositoryDbContext _dbContext, ILogger<ClientRepository> logger, IChangeDetector changeDetector)
            :base(_dbContext,logger,changeDetector)
        {
          
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        public override Task<Client> GetAsync(string id)
        {
            if(id.IsNull())
                return Task.FromResult<Client>(null);
            var client = this.dbContext.Clients
                   .Include(x => x.Claims)
                   .Include(x => x.Properties)
                   .Include(x => x.Secrets)
                   .Where(f => f.Id == id)
                   .FirstOrDefault();
            return Task.FromResult(client);
        }

      
        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            var count = this.dbContext.Clients.Where(f => f.AllowedCorsOrigins.Contains(origin + ";")).Count();
            return Task.FromResult(count > 0);
        }
    }
}
