using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;

namespace Zop.Application.DtoMapper
{
  public  class ClientClaimMapper : Profile
    {
        public ClientClaimMapper()
        {
            CreateMap<ClientClaim, KeyValuePair<string, string>>(MemberList.Destination)
               .ReverseMap();
        }
       
    }
}
