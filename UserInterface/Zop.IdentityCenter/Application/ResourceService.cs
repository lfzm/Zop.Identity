using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Zop.OrleansClient;
using AutoMapper;

namespace Zop.IdentityCenter.Application
{
    public class ResourceService : IResourceService
    {
        private readonly IOrleansClient client;
        private readonly int  grainKey;
        public ResourceService(IOrleansClient client)
        {
            Random rd = new Random();
            grainKey = rd.Next(-50000, 0);
            this.client = client;
        }
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var service = client.GetGrain<Zop.Identity.IApiResourceService>(grainKey);
            var r = await service.FindApiResourceAsync(name);
            return Mapper.Map<ApiResource>(r);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var service = client.GetGrain<Zop.Identity.IApiResourceService>(grainKey);
            var r = await service.FindApiResourcesByScopeAsync(scopeNames);
            return Mapper.Map<IEnumerable<ApiResource>>(r);
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var service = client.GetGrain<Zop.Identity.IIdentityResourceService>(grainKey);
            var r = await service.FindIdentityResourcesByScopeAsync(scopeNames);
            return Mapper.Map<IEnumerable<IdentityResource>>(r);
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiService = client.GetGrain<Zop.Identity.IApiResourceService>(grainKey);
            var identityService = client.GetGrain<Zop.Identity.IIdentityResourceService>(grainKey);

            var identitys = await identityService.GetAllAsync();
            var apis = await apiService.GetAllAsync();

            var result = new Resources(
                identitys.ToArray().Select(x => Mapper.Map<IdentityResource>(x)).AsEnumerable(),
                apis.ToArray().Select(x => Mapper.Map<ApiResource>(x)).AsEnumerable());

            return result;
        }
    }
}
