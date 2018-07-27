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
            ClientAddRequestDto dto = new ClientAddRequestDto
            {
                ClientName = "OTC客户端",
                ClientUri = "https://otc1.zgzop.com",
                MerchantId = "10000",
                Description = "OTC客户端",
                LoginUri = "https://otc1.zgzop.com/login"
            };
            dto.AllowedGrantTypes.Add(GrantType.ClientCredentials);
            dto.AllowedScopes.Add("profile");
            dto.AllowedScopes.Add("phone");
            dto.AllowedScopes.Add("openid");
            dto.AllowedScopes.Add("UC_API");
            dto.AllowedScopes.Add("OTC_API");
            dto.AllowedScopes.Add("OT_API");
            var secret = new DTO.SecretDto("Gcyayx5tX2x12Vhp07BJuoAqXG8P4mUY", "OTC客户端-Gcyayx5tX2x12Vhp07BJuoAqXG8P4mUY", Convert.ToDateTime("2030-01-01 12:12:12"));
            dto.Secrets.Add(secret);
            var response = await service.AddAsync(dto);

      
        }


    }
}
