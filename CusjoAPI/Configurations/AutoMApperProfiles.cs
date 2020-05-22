using AutoMapper;
using CusjoAPI.Data;
using CusjoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CusjoAPI.Configurations
{
    public  class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PermissionDto, User>();
        }
    }
}
