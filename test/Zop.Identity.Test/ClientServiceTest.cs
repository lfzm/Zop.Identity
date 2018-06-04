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
            dto.ClientName = "管理后台";
            dto.ClientUri = "";
            dto.MerchantId = "10000";
            dto.Description = "管理后台";
            dto.AllowedGrantTypes.Add(GrantType.ClientCredentials);
            dto.AllowedScopes.Add("profile");
            dto.AllowedScopes.Add("phone");
            dto.AllowedScopes.Add("openid");
            dto.AllowedScopes.Add("UC_API");
            dto.AllowedScopes.Add("PAY_API");
            dto.AllowedScopes.Add("FM_API");
            var secret = new DTO.SecretDto("CHObfNO6g0serdOVt3Hy4RgPzABFV5a6", "管理后台-CHObfNO6g0serdOVt3Hy4RgPzABFV5a6", Convert.ToDateTime("2030-01-01 12:12:12"));
            dto.Secrets.Add(secret);
            var response = await service.AddAsync(dto);
        }
    }
}
