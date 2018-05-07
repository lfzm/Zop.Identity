using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Zop.OrleansClient;
using AutoMapper;

namespace Zop.IdentityCenter.Application
{
    public class ClientService : IClientService
    {
        private readonly IOrleansClient client;
        public ClientService(IOrleansClient client)
        {
            this.client = client;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var service = client.GetGrain<Zop.Identity.IClientService>(clientId);
            var cli = await service.GetAsync();
            if (cli == null)
                return null;
            return Mapper.Map<Client>(cli);
        }

        public Task<bool> IsOriginAllowedAsync(string origin)
        {
            var service = client.GetGrain<Zop.Identity.IClientService>(Guid.NewGuid().ToString());
            return service.IsOriginAllowedAsync(origin);
        }
    }
}
