using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Zop.OrleansClient;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Zop.IdentityCenter.Application
{
    public class ClientService : IClientService
    {
        private readonly IOrleansClient client;
        private readonly ILogger logger;
        public ClientService(IOrleansClient client, ILogger<ClientService> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            if (clientId.IsNull())
                return null;
            var service = client.GetGrain<Zop.Identity.IClientService>(clientId, OrleansClient.AccessTokenType.NotCredentials);
            var cli = await service.GetAsync();
            if (cli == null)
                return null;

            this.logger.LogDebug(cli.ToJsonString());
            return Mapper.Map<Client>(cli);
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            var service = client.GetGrain<Zop.Identity.IClientService>(Guid.NewGuid().ToString());
            return service.IsOriginAllowedAsync(origin);
        }
    }
}
