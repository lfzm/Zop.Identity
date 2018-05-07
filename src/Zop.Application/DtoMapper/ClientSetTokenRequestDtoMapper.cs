using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO.Request;

namespace Zop.Application.DtoMapper
{
   public class ClientSetTokenRequestDtoMapper : Profile
    {
        public ClientSetTokenRequestDtoMapper()
        {
            base.CreateMap<ClientSetTokenRequestDto, Client>(MemberList.Destination).ReverseMap();
        }
    }
}
