using AutoMapper;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zop.Extensions.OrleansClient;
using Zop.Identity.DTO;

namespace Zop.IdentityCenter.Application
{
    public class PersistedGrantService : IPersistedGrantService
    {
        private readonly IOrleansClient client;
        private readonly ILogger logger;
        public PersistedGrantService(IOrleansClient client, ILogger<PersistedGrantService> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(Guid.NewGuid().ToString());
            var r = await service.GetAllAsync(subjectId);
            return Mapper.Map<IEnumerable<PersistedGrant>>(r);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(key);
            var r = await service.GetAsync();
            return Mapper.Map<PersistedGrant>(r);
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            logger.LogDebug("removing  persisted grants from database for subject {subjectId}, clientId {clientId}", subjectId, clientId);
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(Guid.NewGuid().ToString());
            var r= await  service.RemoveAllAsync(subjectId, clientId);
            if(!r.Success)
                logger.LogInformation("removing persisted grants from database for subject {subjectId}, clientId {clientId}: {SubMsg}",  subjectId, clientId, r.SubMsg);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(Guid.NewGuid().ToString());
            return service.RemoveAllAsync(subjectId, clientId, type);
        }

        public async  Task RemoveAsync(string key)
        {
            logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(key);
            var r=await service.RemoveAsync();
            if (!r.Success)
                logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {SubMsg}", key, r.SubMsg);
            
        }

        public async  Task StoreAsync(PersistedGrant token)
        {
            var service = client.GetGrain<Zop.Identity.IPersistedGrantService>(token.Key);
            var r=await service.StoreAsync(Mapper.Map<PersistedGrantDto>(token));
            if(!r.Success)
                logger.LogWarning("exception updating {persistedGrantKey} persisted grant in database: {SubMsg}", token.Key, r.SubMsg);

        }
    }
}
