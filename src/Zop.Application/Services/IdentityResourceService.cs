using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Authorization;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zop.Application.DataStore;
using Zop.Domain.Entities;
using Zop.Identity;
using Zop.Identity.DTO;
using Zop.Repositories;
using Zop.DTO;

namespace Zop.Application.Services
{
    [Authorize]
    [StorageProvider(ProviderName = RepositoryStorage.DefaultName)]
    public class IdentityResourceService : ApplicationService<IdentityResource>, IIdentityResourceService
    {
        public async Task<ResultResponseDto> AddAsync(IdentityResourceAddRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            IdentityResource identityResource = Mapper.Map<IdentityResource>(dto);
            if (!identityResource.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            IIdentityResourceDataStore dataStore = this.ServiceProvider.GetRequiredService<IIdentityResourceDataStore>();
            if (await dataStore.GetIdAsync(identityResource.Name) > 0)
                return Result.ReFailure<ResultResponseDto>("认证资源名称以存在。", ResultCodes.InvalidParameter);

            base.State = identityResource;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        [AllowAnonymous]
        public async Task<IEnumerable<IdentityResourceDto>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null || scopeNames.Count() == 0)
                return new List<IdentityResourceDto>();
            //前往数据商店获取对应的数据
            var resources = await base.ServiceProvider.GetRequiredService<IIdentityResourceDataStore>().GetListAsync(scopeNames);
            if (resources == null || resources.Count() == 0)
                return new List<IdentityResourceDto>();
            return resources.Select(f => Mapper.Map<IdentityResourceDto>(f)).ToList();
        }
        [AllowAnonymous]
        public async Task<IEnumerable<IdentityResourceDto>> GetAllAsync()
        {
            //前往数据商店获取对应的数据
            var resources = await base.ServiceProvider.GetRequiredService<IIdentityResourceDataStore>().GetAllAsync();
            if (resources == null || resources.Count == 0)
                return new List<IdentityResourceDto>();
            return resources.Select(f => Mapper.Map<IdentityResourceDto>(f)).ToList();
        }
        public async Task<IdentityResourceDto> GetAsync()
        {
            if (base.State == null)
                return null;
            return await Task.FromResult(
                Mapper.Map<IdentityResourceDto>(base.State));
        }
        public async Task<ResultResponseDto> Modify(IdentityResourceModifyRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("认证资源不存在", ResultCodes.NotFound);

            IdentityResource identityResource = base.State.Clone<IdentityResource>();
            Mapper.Map(dto, identityResource);
            if (!identityResource.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            base.State = identityResource;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetClaims(List<string> claims)
        {
            if (!claims.IsNullOrAuy())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("认证资源不存在", ResultCodes.NotFound);

            base.State.SetUserClaims(s => s.SetValue(claims));
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
    }
}
