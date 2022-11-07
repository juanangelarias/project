using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IUserPasswordRepository: IBaseRepository<UserPassword, UserPasswordDto>
{
    Task<UserPasswordDto?> GetCurrentPassword(long userId);
    Task<bool> PasswordUsedBefore(long userId, string hashedPassword);
}