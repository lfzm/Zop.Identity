using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
   public class IdentityTokenDtoMapper : Profile
    {
        public IdentityTokenDtoMapper()
        {
            base.CreateMap<IdentityToken, IdentityTokenDto>(MemberList.Destination)
                .ForMember(f=>f.Data,opt=>opt.MapFrom(s=>s.Data.ToFromJson<Dictionary<string,string>>()))
                .ForMember(f=>f.Key,opt=>opt.MapFrom(s=>s.Id));
        }
    }
}
