using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
   public class PersistedGrantDtoMapper : Profile
    {
        public PersistedGrantDtoMapper()
        {
            base.CreateMap<PersistedGrant, PersistedGrantDto>(MemberList.Destination)
                .ForMember(f=>f.Key,opt=>opt.MapFrom(s=>s.Id))
                .ReverseMap();
        }
    }
}
