using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Orleans;
using Orleans.Runtime;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zop.Repositories.Test
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.TryAddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            services.AddRepositoryStorage(builer =>
            {
                builer.AddIdentityRepository(opt =>
                {
                    opt.DbContextOptions = dbcontext => dbcontext.UseMySql("Server=127.0.0.1;database=test;uid=root;pwd=sapass;");
                });
            });
          
            services.AddLogging();
            return services.BuildServiceProvider();
        }


    }
}
