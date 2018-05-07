using AutoMapper;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zop.Identity.DTO;

namespace Zop.IdentityCenter.Application.DtoMapper
{
    public class PersistedGrantMapper : Profile
    {
        public PersistedGrantMapper()
        {
            base.CreateMap<PersistedGrantDto, PersistedGrant>(MemberList.Destination).ReverseMap();
        }
    }
}
