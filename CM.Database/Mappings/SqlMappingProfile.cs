using CM.Entities;
using CM.Model.Dto;

namespace CM.Database.Mappings;

public class SqlMappingProfile: AutoMapper.Profile
{
    public SqlMappingProfile()
    {
        // C
        CreateMap<Club, ClubDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        
        // M
        CreateMap<Member, MemberDto>().ReverseMap();
        
        // R
        CreateMap<Role, RoleDto>().ReverseMap();
        
        // U
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserPassword, UserPasswordDto>().ReverseMap();
        CreateMap<UserRefreshToken, UserRefreshTokenDto>().ReverseMap();
        CreateMap<UserRequestToken, UserRequestTokenDto>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
    }
}