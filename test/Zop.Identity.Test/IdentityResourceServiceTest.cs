using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Identity.Test
{
    [TestClass]
    public class IdentityResourceServiceTest
    {
        [TestMethod]
        public async Task AddIdentityResource()
        {
            var service = Startup.CreateCluster().GrainFactory.GetGrain<IIdentityResourceService>(0);
            var request = new DTO.IdentityResourceAddRequestDto()
            {
                Name= "email",
                Description = "邮箱地址",
                DisplayName= "邮箱地址"
            };
            request.UserClaims.Add("email");

            var r = await service.AddAsync(request);
        }

        /// <summary>
        /// 获取身份认证资源
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetIdentityResource()
        {
            var service = Startup.CreateCluster().GrainFactory.GetGrain<IIdentityResourceService>(1);
            var r = await service.GetAsync();
        }
    }
}
