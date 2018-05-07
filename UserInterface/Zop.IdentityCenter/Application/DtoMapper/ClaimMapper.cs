using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zop.IdentityCenter.Application.DtoMapper
{
    public class ClaimMapper : Profile
    {
        public ClaimMapper()
        {
            CreateMap<KeyValuePair<string, string>, Claim>(MemberList.Destination)
               .ConstructUsing(src => new Claim(src.Key, src.Value))
               .ReverseMap();
        }
    }



}
