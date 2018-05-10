using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orleans.TestingHost;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Zop.Identity.DTO;

namespace Zop.Identity.Test
{
    [TestClass]
    public class ClientService
    {
        [TestMethod]
        public async Task GetClientAsync()
        {
            IClientService service = Startup.CreateCluster().GrainFactory.GetGrain<IClientService>("162414388953878528");
            var data = await service.GetAsync();
        }
        [TestMethod]
        public async Task AddClientAsync()
        {
            string a = "secret".Sha512();

            IClientService service = Startup.CreateCluster().GrainFactory.GetGrain<IClientService>("@");
            ClientAddRequestDto dto = new ClientAddRequestDto();
            dto.ClientName = "认证中心测试";
            dto.ClientUri = "";
            dto.MerchantId = "0";
            dto.Description = "认证中心对接认证服务的客户端";
            dto.AllowedGrantTypes.Add(GrantType.Hybrid);
            dto.AllowedGrantTypes.Add(GrantType.ClientCredentials);
            dto.AllowedScopes.Add("IDS_API");
            dto.AllowedScopes.Add("offline_access");
            dto.AllowedScopes.Add("openid");
            dto.AllowedScopes.Add("profile");
            dto.AllowedScopes.Add("phone");
            dto.Secrets.Add(new SecretDto("123123".Sha256()));
            var response = await service.AddAsync(dto);
        }
    }
}
