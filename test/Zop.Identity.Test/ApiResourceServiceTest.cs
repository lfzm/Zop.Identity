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
        public async Task GetApiResourceAsync()
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
        public async Task AddApiResourceAsync()
        {
            IApiResourceService service = Startup.CreateCluster().GrainFactory.GetGrain<IApiResourceService>(0);
            var request = new DTO.ApiResourceAddRequestDto()
            {
                Description = "测试",
                DisplayName = "显示",
                Name = "Api"
            };
            request.Secrets.Add(new DTO.SecretDto("ad"));
            var r = await  service.AddAsync(request);
        }
    }
}
