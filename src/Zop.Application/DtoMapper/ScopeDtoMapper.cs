using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
    public class ScopeDtoMapper : Profile
    {
        public ScopeDtoMapper()
        {
            base.CreateMap<ApiScope, ScopeDto>(MemberList.Destination)
                .ForMember(f => f.UserClaims, opt => opt.MapFrom(s => s.GetUserClaims()));
            base.CreateMap<ScopeDto, ApiScope>(MemberList.Destination)
                .ForMember(f => f.UserClaims, opt => opt.MapFrom(s => "".SetValue(s.UserClaims.ToList(),';')));
        }
    }
}
