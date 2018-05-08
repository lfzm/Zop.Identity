using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Orleans.Authorization;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Identity;
using Zop.Identity.DTO;
using Zop.Repositories;
using Zop.Application.DataStore;
using Zop.DTO;

namespace Zop.Application.Services
{
    [Authorize]
    [StorageProvider(ProviderName = RepositoryStorage.DefaultName)]
    public class ApiResourceService : ApplicationService<ApiResource>, IApiResourceService
    {
        public async Task<ResultResponseDto> AddAsync(ApiResourceAddRequestDto dto)
        {
            var result = dto.ValidResult();
            if (!result.Success)
                return Result.ReFailure<ResultResponseDto>(result);
            ApiResource apiResource = new ApiResource(dto.Name, dto.DisplayName, dto.UserClaims);
            apiResource.Description = dto.Description;

            //获取数据商店
            IApiResourceDataStore dataStore = this.ServiceProvider.GetRequiredService<IApiResourceDataStore>();
            if (await dataStore.GetIdAsync(apiResource.Name) > 0)
                return Result.ReFailure<ResultResponseDto>(apiResource.Name + " Api资源已经存在", ResultCodes.InvalidParameter);
            //添加秘钥
            foreach (var item in dto.Secrets)
            {
                ApiSecret secret = Mapper.Map<ApiSecret>(item);
                if (!secret.IsValid())
                    return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
                apiResource.Secrets.Add(secret);
            }
            //添加包含范围
            foreach (var item in dto.Scopes)
            {
                ApiScope scope = Mapper.Map<ApiScope>(item);
                if (!scope.IsValid())
                    return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
                //判断数据库以及当前Api资源是否存在当前Api范围
                if (await dataStore.GetScopeIdAsync(scope.Name) > 0 || apiResource.Scopes.ToList().Exists(f => f.Name == scope.Name))
                    return Result.ReFailure<ResultResponseDto>(scope.Name + "Api范围已经存在", ResultCodes.InvalidParameter);
                apiResource.Scopes.Add(scope);
            }

            if (!apiResource.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误，Api资源不合法", ResultCodes.InvalidParameter);
            base.State = apiResource;
            await base.WriteStateAsync();
            base.DeactivateOnIdle();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> AddScope(ScopeDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.InvalidParameter);

            ApiScope scope = Mapper.Map<ApiScope>(dto);
            if (!scope.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);

            IApiResourceDataStore dataStore = this.ServiceProvider.GetRequiredService<IApiResourceDataStore>();
            if (await dataStore.GetScopeIdAsync(scope.Name) > 0)
                return Result.ReFailure<ResultResponseDto>(scope.Name + "Api范围已经存在", ResultCodes.InvalidParameter);
            this.State.Scopes.Add(scope);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();

        }
        public async Task<ResultResponseDto> AddSecret(SecretDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.NotFound);

            ApiSecret secret = Mapper.Map<ApiSecret>(dto);
            this.State.Secrets.Add(secret);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ApiResourceDto> FindApiResourceAsync(string name)
        {
            if (name.IsNull())
                return null;
            //根据Name获取对应的Id
            int id = await base.ServiceProvider.GetRequiredService<IApiResourceDataStore>().GetIdAsync(name);
            if (id <= 0)
                return null;
            //通过ID获取数据
            var service = this.GrainFactory.GetStateGrain<ApiResource>(id);
            var data = await service.ReadState();
            if (data == null)
                return null;
            return Mapper.Map<ApiResourceDto>(data);
        }
        public async Task<IEnumerable<ApiResourceDto>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null || scopeNames.Count() == 0)
                return new List<ApiResourceDto>();
            //前往数据商店获取对应的数据
            var resources = await base.ServiceProvider.GetRequiredService<IApiResourceDataStore>().GetListAsync(scopeNames);
            if (resources == null || resources.Count == 0)
                return new List<ApiResourceDto>();
            return resources.Select(f => Mapper.Map<ApiResourceDto>(f)).ToList();
        }
        public async Task<IEnumerable<ApiResourceDto>> GetAllAsync()
        {
            //前往数据商店获取对应的数据
            var resources = await base.ServiceProvider.GetRequiredService<IApiResourceDataStore>().GetAllAsync();
            if (resources == null || resources.Count == 0)
                return new List<ApiResourceDto>();
            return resources.Select(f => Mapper.Map<ApiResourceDto>(f)).ToList();
        }
        public async Task<ApiResourceDto> GetAsync()
        {
            if (base.State == null)
                return null;
            return await Task.FromResult(
                Mapper.Map<ApiResourceDto>(base.State));
        }
        public async Task<ResultResponseDto> Modify(ApiResourceModifyRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.NotFound);
            base.State.DisplayName = dto.DisplayName;
            base.State.Description = dto.Description;
            base.State.Enabled = dto.Enabled;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> ModifyClaims(List<string> claims)
        {
            if (!claims.IsNullOrAuy())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.InvalidParameter);

            base.State.SetUserClaims(s => s.SetValue(claims));
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> ModifyScope(ApiResourceModifyScopeRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.InvalidParameter);


            var apiResource = base.State.Clone<ApiResource>();
            var scope = apiResource.Scopes.Where(f => f.Name == dto.Name).FirstOrDefault();
            if (scope == null)
                return Result.ReFailure<ResultResponseDto>(dto.Name + "API范围不存在", ResultCodes.InvalidParameter);

            Mapper.Map((ScopeDto)dto, scope);
            if (!scope.IsValid())
                return Result.ReFailure<ResultResponseDto>("API范围数据不合法", ResultCodes.InvalidParameter);

            base.State = apiResource;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> ModifySecret(ApiResourceModifySecretRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.NotFound);

            var apiResource = base.State.Clone<ApiResource>();
            var secret = apiResource.Secrets.Where(f => f.Id == dto.Id).FirstOrDefault();
            if (secret == null)
                return Result.ReFailure<ResultResponseDto>("API秘钥不存在", ResultCodes.NotFound);

            Mapper.Map((SecretDto)dto, secret);
            if (!secret.IsValid())
                return Result.ReFailure<ResultResponseDto>("API秘钥数据不合法", ResultCodes.InvalidParameter);

            base.State = apiResource;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> RemoveScope(string name)
        {
            if (name.IsNull())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.InvalidParameter);
            //找到对应Api范围
            var scope = base.State.Scopes.ToList().FirstOrDefault(f => f.Name == name);
            if (scope == null)
                return Result.ReFailure<ResultResponseDto>("API范围不存在", ResultCodes.InvalidParameter);

            base.State.Scopes.Remove(scope);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> RemoveSecret(int id)
        {
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("API资源不存在", ResultCodes.NotFound);
            //找到对应Api范围
            var secret = base.State.Secrets.ToList().FirstOrDefault(f => f.Id == id);
            if (secret == null)
                return Result.ReFailure<ResultResponseDto>("API秘钥不存在", ResultCodes.NotFound);

            base.State.Secrets.Remove(secret);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
    }
}
