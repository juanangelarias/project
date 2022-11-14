using System.Security.Claims;
using CM.Model.Dto;
using CM.Model.General;

namespace CM.Core.Authentication;

public interface IJwtGenerator
{
    Task<Token> GetToken(long userId);
    ClaimsPrincipal? ValidateToken(string? token);
    UserRefreshTokenDto GenerateRefreshToken(string ipAddress);
}