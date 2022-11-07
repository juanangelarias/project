using System.Security.Claims;
using CM.Model.Dto;

namespace CM.Core.Authentication;

public interface IJwtGenerator
{
    Task<string> GetToken(long userId);
    ClaimsPrincipal? ValidateToken(string? token);
    UserRefreshTokenDto GenerateRefreshToken(string ipAddress);
}