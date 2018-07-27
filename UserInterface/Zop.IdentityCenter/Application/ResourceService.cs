using AutoMapper;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Extensions.OrleansClient;
using Zop.Identity;

namespace Zop.IdentityCenter.Application
{
    public class ResourceService : IResourceService
    {
        private readonly IOrleansClient client;
        private readonly ILogger logger;
        public ResourceService(IOrleansClient client, ILogger<ResourceService> _logger)
        {
            this.client = client;
            this.logger = _logger;
        }
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var service = client.GetGrain<IApiResourceService>(0);
            var r = await service.FindApiResourceAsync(name);
            return Mapper.Map<ApiResource>(r);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var service = client.GetGrain<IApiResourceService>(0);
            var r = await service.FindApiResourcesByScopeAsync(scopeNames.ToList());
            return Mapper.Map<IEnumerable<ApiResource>>(r);
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            this.logger.LogError(JsonConvert.SerializeObject(scopeNames));
            var service = client.GetGrain<IIdentityResourceService>(0);
            this.logger.LogError("获取Grain");
            var r = await service.FindIdentityResourcesByScopeAsync(scopeNames.ToList());
            this.logger.LogError(JsonConvert.SerializeObject(r));
            return Mapper.Map<IEnumerable<IdentityResource>>(r);
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiService = client.GetGrain<IApiResourceService>(0);
            var identityService = client.GetGrain<IIdentityResourceService>(0);

            var identitys = await identityService.GetAllAsync();
            var apis = await apiService.GetAllAsync();

            var result = new Resources(
                identitys.ToArray().Select(x => Mapper.Map<IdentityResource>(x)).AsEnumerable(),
                apis.ToArray().Select(x => Mapper.Map<ApiResource>(x)).AsEnumerable());

            return result;
        }
    }
}
