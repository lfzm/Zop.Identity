using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
    public class ClientDtoMapper : Profile
    {
        public ClientDtoMapper()
        {
            base.CreateMap<ClientClaim, ClaimDto>(MemberList.Destination)
                .ReverseMap();
            base.CreateMap<ClientProperty, KeyValuePair<string, string>>()
                .ReverseMap();
            base.CreateMap<Client, ClientDto>(MemberList.Destination)
                .ForMember(f => f.ClientId, opt => opt.MapFrom(s => s.Id))
                .ForMember(f => f.ClientSecrets, opt => opt.MapFrom(s => s.Secrets))
                .ForMember(f => f.AllowedGrantTypes, opt => opt.MapFrom(s => s.GetAllowedGrantTypes()))
                .ForMember(f => f.RedirectUris, opt => opt.MapFrom(s => s.GetRedirectUris()))
                .ForMember(f => f.AllowedScopes, opt => opt.MapFrom(s => s.GetAllowedScopes()))
                .ForMember(f => f.PostLogoutRedirectUris, opt => opt.MapFrom(s => s.GetPostLogoutRedirectUris()))
                .ForMember(f => f.IdentityProviderRestrictions, opt => opt.MapFrom(s => s.GetIdentityProviderRestrictions()))
                .ForMember(f => f.AllowedCorsOrigins, opt => opt.MapFrom(s => s.GetAllowedCorsOrigins()))
                .ReverseMap();
        }
    }
}
