using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Authorization;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Application.DataStore;
using Zop.Domain.Entities;
using Zop.DTO;
using Zop.Identity;
using Zop.Identity.DTO;
using Zop.Repositories;

namespace Zop.Application.Services
{
    [Authorize]
    [StorageProvider(ProviderName = RepositoryStorage.DefaultName)]
    public class PersistedGrantService : ApplicationService<PersistedGrant>, IPersistedGrantService
    {
        private readonly ILogger Logger;
        public PersistedGrantService(ILogger<PersistedGrantService> _logger)
        {
            this.Logger = _logger;
        }
        public async Task<IList<PersistedGrantDto>> GetAllAsync(string subjectId)
        {
            if (subjectId.IsNull())
                return new List<PersistedGrantDto>();
            //通过数据商店获取数据
            var resources = await base.ServiceProvider.GetRequiredService<IPersistedGrantDataStore>().GetListAsync(subjectId);
            if (resources == null || resources.Count == 0)
                return new List<PersistedGrantDto>();
            return resources.Select(f => Mapper.Map<PersistedGrantDto>(f)).ToList();
        }
        public async Task<PersistedGrantDto> GetAsync()
        {
            if (base.State == null)
                return null;
            return await Task.FromResult(
                Mapper.Map<PersistedGrantDto>(base.State));
        }
        public async Task<ResultResponseDto> RemoveAllAsync(string subjectId, string clientId)
        {
            //通过数据商店获取数据
            var keys = await base.ServiceProvider.GetRequiredService<IPersistedGrantDataStore>().GetKeysAsync(subjectId, clientId);
            if (keys == null || keys.Count == 0)
                return Result.ReSuccess<ResultResponseDto>();

            foreach (var key in keys)
            {
                await base.GrainFactory.GetGrain<IPersistedGrantService>(key).RemoveAsync();
            }
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> RemoveAllAsync(string subjectId, string clientId, string type)
        {
            //通过数据商店获取数据
            var keys = await base.ServiceProvider.GetRequiredService<IPersistedGrantDataStore>().GetKeysAsync(subjectId, clientId, type);
            if (keys == null || keys.Count == 0)
                return Result.ReSuccess<ResultResponseDto>();

            foreach (var key in keys)
            {
                await base.GrainFactory.GetGrain<IPersistedGrantService>(key).RemoveAsync();
            }
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> RemoveAsync()
        {
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("发放令牌已经不存在",ResultCodes.NotFound);
            await base.ClearAsync();
            base.DeactivateOnIdle();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> StoreAsync(PersistedGrantDto dto)
        {
            if (!dto.IsValid(Logger, LogLevel.Error))
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            //转换为存储对象
            PersistedGrant grant = null;
            if (base.State == null)
            {
                //添加令牌
                grant = Mapper.Map<PersistedGrant>(dto);
                grant.SetId(this.GetPrimaryKeyString());
            }
            else
            {
                grant = base.State.Clone<PersistedGrant>();
                grant = Mapper.Map(dto, grant);
            }
            if (!grant.IsValid(Logger, LogLevel.Error))
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            base.State = grant;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
    }
}
