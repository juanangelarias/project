using CM.Entities;
using CM.Model.Dto;

namespace CM.Database.Mappings;

public class SqlMappingProfile : AutoMapper.Profile
{
    public SqlMappingProfile()
    {
        // C
        CreateMap<Club, ClubDto>().ReverseMap();
        CreateMap<ClubType, ClubTypeDto>().ReverseMap();
        CreateMap<Conference, ConferenceDto>().ReverseMap();
        CreateMap<ConferencePaymentMethod, ConferencePaymentMethodDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();
        CreateMap<Currency, CurrencyDto>().ReverseMap();

        // M
        CreateMap<Member, MemberDto>().ReverseMap();

        // P
        CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductCombo, ProductComboDto>().ReverseMap();
        CreateMap<Program, ProgramDto>().ReverseMap();

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
