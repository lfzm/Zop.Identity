using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Repositories
{
    public class TestRepository : Repository<Test, int>
    {
        public override Task DeleteAsync(Test entity)
        {
            return Task.CompletedTask;
        }

        public override Task<Test> GetAsync(int id)
        {
            Test t =  null;
            return Task.FromResult(t);
        }

        public override Task<Test> InsertAsync(Test entity)
        {
            entity.Id = 123;
            return Task.FromResult(entity);
        }

        public override Task<Test> UpdateAsync(Test entity)
        {
            return Task.FromResult(entity);
        }
    }
}
