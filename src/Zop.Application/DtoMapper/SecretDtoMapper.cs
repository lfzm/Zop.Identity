using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;
using Zop.Identity.DTO;

namespace Zop.Application.DtoMapper
{
    public class SecretDtoMapper : Profile
    {
        public SecretDtoMapper()
        {
            base.CreateMap<ApiSecret, SecretDto>(MemberList.Destination).ReverseMap();
            base.CreateMap<SecretDto, ClientSecret>(MemberList.Destination).ReverseMap();

        }
    }
}
