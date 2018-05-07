using Orleans.Authorization;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Zop.Domain.Entities;
using Zop.Identity;
using Zop.Repositories;
using System.Threading.Tasks;
using Zop.DTO;
using Zop.Identity.DTO;
using Zop.Toolkit.IDGenerator;
using AutoMapper;

namespace Zop.Application.Services
{
    [Authorize]
    [StorageProvider(ProviderName = RepositoryStorage.DefaultName)]
    public class IdentityTokenService : ApplicationService<IdentityToken>, IIdentityTokenService
    {
        public Task<IdentityTokenDto> GetAsync()
        {
            if (base.State == null)
                return Task.FromResult<IdentityTokenDto>(null);
            if (base.State.ValidityTime < DateTime.Now)
                return Task.FromResult<IdentityTokenDto>(null);
            return Task.FromResult(
                Mapper.Map<IdentityTokenDto>(base.State));
        }

        public async Task<IdentityTokenAddResponseDto> StoreAsync(IdentityTokenAddRequestDto dto)
        {
            var result = dto.ValidResult();
            if (!result.Success)
                return Result.ReFailure<IdentityTokenAddResponseDto>(result);

            //生成Token Key
            string key = base.ServiceProvider.GetRequiredService<IIDGenerated>().NextId().ToString();
            key = key.Sha256();
            IdentityToken token = new IdentityToken(key, dto.SubjectId)
            {
                ClientId = dto.ClientId,
                Data = dto.Data.ToJsonString(),
                IdentityIP4 = dto.IdentityIP4,
                Type = dto.Type,
                ValidityTime = dto.ValidityTime,
                ReturnUrl = dto.ReturnUrl
            };
            base.State = token;
            await base.WriteStateAsync();
            base.DeactivateOnIdle();
            return new IdentityTokenAddResponseDto(token.Id);
        }
    }
}
