using CM.Entities;
using CM.Model.Dto;

namespace CM.Database.Mappings;

public class SqlMappingProfile : AutoMapper.Profile
{
    public SqlMappingProfile()
    {
        // C
        CreateMap<Club, ClubDto>()
            .ReverseMap();
        CreateMap<ClubType, ClubTypeDto>().ReverseMap();
        CreateMap<Conference, ConferenceDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Currency, CurrencyDto>().ReverseMap();

        // M
        CreateMap<Member, MemberDto>()
            .ReverseMap();

        // P
        CreateMap<Product, ProductDto>();
        CreateMap<ProductCombo, ProductComboDto>();
        CreateMap<Program, ProgramDto>();

        // R
        CreateMap<Role, RoleDto>()
            .ReverseMap();

        // U
        CreateMap<User, UserDto>()
            .ReverseMap();
        CreateMap<UserPassword, UserPasswordDto>().ReverseMap();
        CreateMap<UserRefreshToken, UserRefreshTokenDto>().ReverseMap();
        CreateMap<UserRequestToken, UserRequestTokenDto>().ReverseMap();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();
    }
}
