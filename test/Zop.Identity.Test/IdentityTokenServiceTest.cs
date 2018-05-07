using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Application.Services;

namespace Zop.Identity.Test
{
    [TestClass]
    public class IdentityTokenServiceTest
    {
        [TestMethod]
        public async Task AddIdentityToken()
        {
            IIdentityTokenService service = Startup.CreateCluster().GrainFactory.GetGrain<IIdentityTokenService>(Guid.NewGuid().ToString());
            var request = new DTO.IdentityTokenAddRequestDto()
            {
                ClientId = "a",
                IdentityIP4 = "127.0.0.1",
                SubjectId = "a",
                Type = "a"
            };
            request.Data.Add("sud", "1");
            var r = await service.StoreAsync(request);
        }

        [TestMethod]
        public async Task GetIdentityTokenDto()
        {
            IIdentityTokenService service = Startup.CreateCluster().GrainFactory.GetGrain<IIdentityTokenService>("8eXxTAu2f94e7y2pC7nqJr4nBgnRh5YY4JcqZTuZrjk=");
            var r = await service.GetAsync();
        }
    }
}
