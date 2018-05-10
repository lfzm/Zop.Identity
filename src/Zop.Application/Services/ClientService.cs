using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Authorization;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Application.DataStore;
using Zop.Domain.Entities;
using Zop.Identity;
using Zop.Identity.DTO;
using Zop.Repositories;
using Zop.DTO;
using Zop.Identity.DTO.Request;
using System.Linq;
using Zop.Toolkit.IDGenerator;

namespace Zop.Application.Services
{
    [Authorize]
    [StorageProvider(ProviderName = RepositoryStorage.DefaultName)]
    public class ClientService : ApplicationService<Client>, IClientService
    {
        public async Task<ResultResponseDto> AddAsync(ClientAddRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            //生成
            string clientId = base.ServiceProvider.GetRequiredService<IIDGenerated>().NextId().ToString();
            Client client = new Client(clientId, dto.MerchantId);
            client.ClientName = dto.ClientName;
            client.ClientUri = dto.ClientUri;
            client.LogoUri = dto.LogoUri;
            client.Description = dto.Description;
            var result = this.SetScopes(dto.AllowedScopes, client);
            if (!result.Success)
                return Result.ReFailure<ResultResponseDto>(result);

            result = this.SetGrantTypes(dto.AllowedGrantTypes, client);
            if (!result.Success)
                return Result.ReFailure<ResultResponseDto>(result);

            foreach (var item in dto.Secrets)
            {
                Secret secret = Mapper.Map<Secret>(item);
                if (!secret.IsValid())
                    return Result.ReFailure<ResultResponseDto>("秘钥请求参数错误", ResultCodes.InvalidParameter);
                client.Secrets.Add(secret);
            }
            if (!client.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误不合法", ResultCodes.InvalidParameter);
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();

        }
        public async Task<ResultResponseDto> AddSecrets(SecretDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);

            Secret secret = Mapper.Map<Secret>(dto);
            if (!secret.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            base.State.Secrets.Add(secret);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        [AllowAnonymous]
        public Task<ClientDto> GetAsync()
        {
            if (base.State==null)
                return Task.FromResult<ClientDto>(null);
            ClientDto client = Mapper.Map<ClientDto>(base.State);
            return Task.FromResult(client);
        }
        public Task<string> GetLoginUrlAsync()
        {
            if (base.State == null)
                return Task.FromResult<string>(null);
            return Task.FromResult(base.State.LoginUri);
        }
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            return await base.ServiceProvider.GetRequiredService<IClientDataStore>().IsOriginAllowedAsync(origin);
        }
        public async Task<ResultResponseDto> RemoveSecrets(int id)
        {
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);
            //找到对应Api范围
            var secret = base.State.Secrets.ToList().FirstOrDefault(f => f.Id == id);
            if (secret == null)
                return Result.ReFailure<ResultResponseDto>("客户端秘钥不存在", ResultCodes.NotFound);

            base.State.Secrets.Remove(secret);
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetAuthConfig(ClientSetAuthRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);

            var client = base.State.Clone<Client>();

            client.SetAllowedCorsOrigins(s => s.Cover(dto.AllowedCorsOrigins));
            client.SetAllowedGrantTypes(s => s.Cover(dto.AllowedGrantTypes));
            client.SetIdentityProviderRestrictions(s => s.Cover(dto.IdentityProviderRestrictions));
            client.SetRedirectUris(s => s.Cover(dto.RedirectUris));
            client.EnableLocalLogin = dto.EnableLocalLogin;
            client.ProtocolType = dto.ProtocolType;
            client.LoginUri = dto.LoginUri;
            this.SetScopes(dto.AllowedScopes, client);
            if (!client.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误不合法", ResultCodes.InvalidParameter);
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetBasics(ClientSetBasicsRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);
            var client = base.State.Clone<Client>();
            List<ClientClaim> claims = Mapper.Map<List<ClientClaim>>(dto.Claims);
            foreach (var claim in claims)
            {
                if (!claim.IsValid())
                    return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
                client.Claims.Add(claim);
            }
            client.ClientName = dto.ClientName;
            client.ClientUri = dto.ClientUri;
            client.LogoUri = dto.LogoUri;
            client.Enabled = dto.Enabled;
            client.Description = dto.Description;
            if (!client.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误不合法", ResultCodes.InvalidParameter);
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetConsentConfig(ClientSetConsentRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);

            base.State.RequireConsent = dto.RequireConsent;
            base.State.AllowRememberConsent = dto.AllowRememberConsent;
            base.State.ConsentLifetime = dto.ConsentLifetime;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();

        }
        public async Task<ResultResponseDto> SetLogoutConfig(ClientSetLogoutRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);

            var client = base.State.Clone<Client>();
            client.FrontChannelLogoutSessionRequired = dto.FrontChannelLogoutSessionRequired;
            client.FrontChannelLogoutUri = dto.FrontChannelLogoutUri;
            client.BackChannelLogoutUri = dto.BackChannelLogoutUri;
            client.BackChannelLogoutSessionRequired = dto.BackChannelLogoutSessionRequired;
            client.SetPostLogoutRedirectUris(f => f.Cover(dto.PostLogoutRedirectUris));

            if (!client.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误不合法", ResultCodes.InvalidParameter);
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetSafetyConfig(ClientSetSafetyRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);

            var client = base.State.Clone<Client>();
            foreach (var item in dto.Secrets)
            {
                Secret secret = Mapper.Map<Secret>(item);
                if (!secret.IsValid())
                    return Result.ReFailure<ResultResponseDto>("秘钥请求参数错误", ResultCodes.InvalidParameter);
                client.Secrets.Add(secret);
            }
            client.RequireClientSecret = dto.RequireClientSecret;
            client.RequirePkce = dto.RequirePkce;
            client.AllowPlainTextPkce = dto.AllowPlainTextPkce;
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }
        public async Task<ResultResponseDto> SetTokenConfig(ClientSetTokenRequestDto dto)
        {
            if (!dto.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误", ResultCodes.InvalidParameter);
            if (base.State == null)
                return Result.ReFailure<ResultResponseDto>("客户端不存在", ResultCodes.NotFound);
            var client = base.State.Clone<Client>();
            Mapper.Map(dto, client);
            if (!client.IsValid())
                return Result.ReFailure<ResultResponseDto>("请求参数错误不合法", ResultCodes.InvalidParameter);
            base.State = client;
            await base.WriteStateAsync();
            return Result.ReSuccess<ResultResponseDto>();
        }

        /// <summary>
        /// 设置授权服务
        /// </summary>
        /// <param name="scopes">需要设置的授权范围</param>
        /// <param name="client">客户端</param>
        private Result SetScopes(List<string> scopes, Client client)
        {
            client.SetAllowedScopes(s => s.Cover(scopes));
            return Result.ReSuccess();
        }

        /// <summary>
        /// 设置授权模式
        /// </summary>
        /// <param name="grantTypes">需要设置的授权模式</param>
        /// <param name="client">客户端</param>
        private Result SetGrantTypes(List<string> grantTypes, Client client)
        {
            client.SetAllowedGrantTypes(s => s.Cover(grantTypes));

            if(client.AllowedGrantTypes.GetList().Contains("hybrid"))
            {
                client.AllowOfflineAccess = true;
            }
            return Result.ReSuccess();
        }

    
    }
}
