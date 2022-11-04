using System.Security.Claims;
using CM.Entities;
using CM.Model.Dto;

namespace CM.Common.Authentication;

public interface IJwtGenerator
{
    Task<string> GetToken(long userId);
    ClaimsPrincipal ValidateToken(string token);
    UserRefreshTokenDto GenerateRefreshToken(string ipAddress);
}