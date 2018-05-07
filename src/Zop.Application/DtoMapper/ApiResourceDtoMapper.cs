using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Zop.Domain.Entities;
using Zop.Identity.DTO;
using System.Linq;

namespace Zop.Application.DtoMapper
{
    public class ApiResourceDtoMapper : Profile
    {
        public ApiResourceDtoMapper()
        {
            base.CreateMap<ApiResource, ApiResourceDto>(MemberList.Destination)
                .ForMember(f=>f.UserClaims,opt=>opt.MapFrom(s=>s.GetUserClaims()))
                .ForMember(f => f.ApiSecrets, opt => opt.MapFrom(s => s.Secrets))
                .ReverseMap();
        }
    }
}
