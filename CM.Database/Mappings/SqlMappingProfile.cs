﻿using CM.Entities;
using CM.Model.Dto;

namespace CM.Database.Mappings;

public class SqlMappingProfile: AutoMapper.Profile
{
    public SqlMappingProfile()
    {
        // R
        CreateMap<Role, RoleDto>().ReverseMap();
        
        // U
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserRefreshToken, UserRefreshTokenDto>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
    }
}