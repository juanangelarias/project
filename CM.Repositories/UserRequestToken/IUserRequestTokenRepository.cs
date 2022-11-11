using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IUserRequestTokenRepository: IBaseRepository<UserRequestToken, UserRequestTokenDto>
{
    Task<UserRequestTokenDto> GetTokenDefinition(string token);
}