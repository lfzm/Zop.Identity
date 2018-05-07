using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orleans.TestingHost;
using System;
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
            IClientService service = Startup.CreateCluster().GrainFactory.GetGrain<IClientService>("@");
            ClientAddRequestDto dto = new ClientAddRequestDto();
            dto.ClientName = "≤‚ ‘";
            dto.ClientUri = "";
            dto.MerchantId = "1";
            dto.Description = "≤‚ ‘";
            dto.AllowedGrantTypes.Add(GrantType.Implicit);
            dto.AllowedGrantTypes.Add(GrantType.AuthorizationCode);
            dto.AllowedGrantTypes.Add(GrantType.Hybrid);
            dto.AllowedScopes.Add("abc");
            dto.Secrets.Add(new SecretDto()
            {
                Expiration = DateTime.Now.AddYears(1),
                Type = SecretTypes.X509CertificateName,
                Value = "123123"
            });
            var response = await service.AddAsync(dto);
        }
    }
}
