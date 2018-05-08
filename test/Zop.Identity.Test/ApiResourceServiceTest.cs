using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            IApiResourceService service = Startup.CreateCluster().GrainFactory.GetGrain<IApiResourceService>(0);
            var request = new DTO.ApiResourceAddRequestDto()
            {
                Description = "认证中心的API",
                DisplayName = "认证中心",
                Name = "IDC_API",
            };
            var secret = new DTO.SecretDto("iuxBz0vhlRgpPRjEKsr4BiwdkmNctZpfmOGCmxcnt3UPO4BwYhfK14g78oDhfRSl0V");
            request.Secrets.Add(secret);
            var r = await  service.AddAsync(request);
        }
    }
}
