using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Zop.Identity.Test
{
    [TestClass]
    public class ApiResourceServiceTest
    {
        /// <summary>
        /// 获取API资源 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetApiResource()
        {
            IApiResourceService service = Startup.CreateCluster().GrainFactory.GetGrain<IApiResourceService>(1);
            var r = await service.GetAsync();
        }

        /// <summary>
        /// 添加API资源
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [TestMethod]
        public async Task AddApiResource()
        {
            string d = "944d6r8wQsImSaty53p12JQ4VdmpMO0r".Sha512();
            IApiResourceService service = Startup.CreateCluster().GrainFactory.GetGrain<IApiResourceService>(0);
            var request = new DTO.ApiResourceAddRequestDto()
            {
                Description = "测试API",
                DisplayName = "测试API",
                Name = "test_api",
            };
            var secret = new DTO.SecretDto("123123");
            request.Secrets.Add(secret);
            var r = await  service.AddAsync(request);
        }
    }
}
