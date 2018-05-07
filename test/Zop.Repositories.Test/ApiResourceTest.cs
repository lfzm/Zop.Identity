using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Orleans.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Zop.Repositories.Configuration;

namespace Zop.Repositories.Test
{
    [TestClass]
    public class ApiResourceTest
    {
        private ApiResourceRepository repository;
        public ApiResourceTest()
        {
            repository = Startup.ConfigureServices().GetService<ApiResourceRepository>();
        }
        [TestMethod]
        public async Task InsertAsync()
        {
            var apiResource = new ApiResource("测试", new List<string>() { "ApiName", "ApiDec" });
            int id = await repository.GetIdAsync("测试");
            if (id == 0)
            {
                var data = await repository.InsertAsync(apiResource);
            }
        }
        [TestMethod]
        public async Task GetAsync()
        {
            var data = await repository.GetAsync(1);
        }
        [TestMethod]
        public async Task GetIdAsync()
        {
            int id = await repository.GetIdAsync("测试");
        }
        [TestMethod]
        public async Task GetAllAsync()
        {
            var data = await repository.GetAllAsync();
        }
        [TestMethod]
        public async Task DeleteAsync()
        {
            var data = await repository.GetAsync(7);
            await repository.DeleteAsync(data);
        }
        [TestMethod]
        public async Task UpdateAsync()
        {
            List<string> data1 = null;
            var data = await repository.GetAsync(7);
            data.SetUserClaims(s => s.SetValue(DateTime.Now.Ticks.ToString()));
            data.Description = DateTime.Now.Ticks.ToString();
            if (data.Scopes.Count > 1)
            {
                data.Scopes[1].DisplayName = DateTime.Now.Ticks.ToString();
                data.Scopes.RemoveAt(0);
            }
            data.Scopes.Add(new ApiScope(DateTime.Now.Ticks.ToString()));
            data.Scopes.Add(new ApiScope(DateTime.Now.Ticks.ToString()));
            var d = await repository.UpdateAsync(data);

            //性能测试
            //var scope1 = Startup.ConfigureServices().CreateScope();
            //var p = scope1.ServiceProvider;
            //var repository1 = p.GetRequiredServiceByName<IRepository>(typeof(ApiResource).Name);
            //var data = (ApiResource)await repository1.ReadAsync(4);
            //DateTime st = DateTime.Now;
            //for (int i = 0; i < 1000; i++)
            //{
            //    var ddd = p.GetRequiredService<RepositoryDbContext>();

            //    repository1 = p.GetRequiredServiceByName<IRepository>(typeof(ApiResource).Name);
            //    data.UserClaims.SetValue(DateTime.Now.Ticks.ToString());
            //    data.Description = DateTime.Now.Ticks.ToString();
            //    if (data.Scopes.Count > 0)
            //    {
            //        data.Scopes[0].DisplayName = DateTime.Now.Ticks.ToString();
            //        data.Scopes.RemoveAt(0);
            //    }
            //    data.Scopes.Add(new ApiScope(DateTime.Now.Ticks.ToString()));
            //    data.Scopes.Add(new ApiScope(DateTime.Now.Ticks.ToString()));
            //    data = (ApiResource)await repository1.WriteAsync(data);
            //    data = data.Clone<ApiResource>();
            //}
            //var ms = (DateTime.Now - st).TotalMilliseconds;
        }
    }
}
