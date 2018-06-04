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
    /// <summary>
    /// 身份令牌信息
    /// </summary>
    public class IdentityTokenRepository : EFRepository<IdentityToken, string>
    {
        public IdentityTokenRepository(RepositoryDbContext dbContext, ILogger<ApiResourceRepository> logger, IChangeDetector changeDetector)
            : base(dbContext, logger, changeDetector)
        {

        }


        public override Task<IdentityToken> GetAsync(string id)
        {
            if (id.IsNull())
                return Task.FromResult<IdentityToken>(null);

            var identityToken = base.dbContext.IdentityTokens.Where(f => f.Id == id).FirstOrDefault();
            return Task.FromResult(identityToken);
        }
    }
}
