using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Zop.Identity.DTO;
using IdentityServer4.Models;
using System.Security.Claims;

namespace Zop.IdentityCenter.Application.DtoMapper
{
    public class ClientMapper : Profile
    {
        public ClientMapper()
        {
            base.CreateMap<ClaimDto, Claim>(MemberList.None)
                .ConstructUsing(src => new Claim(src.Type, src.Value))
                .ReverseMap();
            base.CreateMap<ClientDto, Client>(MemberList.Destination);
        }
    }
}
