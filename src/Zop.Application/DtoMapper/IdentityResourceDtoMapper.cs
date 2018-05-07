using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
    public class IdentityResourceDtoMapper : Profile
    {
        public IdentityResourceDtoMapper()
        {
            base.CreateMap<IdentityResource, IdentityResourceDto>(MemberList.Destination)
                .ForMember(f => f.UserClaims, opt => opt.MapFrom(s => s.GetUserClaims()));

            base.CreateMap<IdentityResourceAddRequestDto, IdentityResource>(MemberList.Destination)
               .ForMember(f => f.UserClaims, opt => opt.MapFrom(s => "".SetValue(s.UserClaims.ToList(), ';')));

            base.CreateMap<IdentityResourceModifyRequestDto, IdentityResource>();
        }
    }
}
