using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CM.Common.Configuration.Models;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CM.Core.Authentication;

public class JwtGenerator : IJwtGenerator
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Token> GetToken(long userId)
    {
        var user = await _userRepository.GetByIdExpandedAsync(userId);
        var claims = GetClaims(user);
        var expiresInMinutes = Convert.ToInt32(_jwtSettings.AccessTokenValidityInMinutes);
        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
        var token = CreateToken(claims, expires);

        var result = new Token
        {
            Value = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = expires
        };

        return result;
    }

    private List<Claim> GetClaims(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("Id", Convert.ToString(user.Id)),
            new("UserName", user.UserName)
        };

        if (user.Roles != null && user.Roles.Any())
        {
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));
        }

        return claims;
    }

    private JwtSecurityToken CreateToken(IEnumerable<Claim> claims, DateTime expires)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key ?? ""));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: expires);
        return token;
    }

    public ClaimsPrincipal? ValidateToken(string? token)
    {
        if (token == null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key ?? "");
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = Convert.ToBoolean(_jwtSettings.ValidateIssuerSigningKey),
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = Convert.ToBoolean(_jwtSettings.ValidateIssuer),
                ValidateAudience = Convert.ToBoolean(_jwtSettings.ValidateAudience),
                ValidateLifetime = Convert.ToBoolean(_jwtSettings.ValidateLifetime),
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                // Set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return claimsPrincipal;
        }
        catch
        {
            // If validation fails return null
            return null;
        }
    }

    public UserRefreshTokenDto GenerateRefreshToken(string ipAddress)
    {
        // generate token that is valid for (n) days
        using var randomNumberProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        randomNumberProvider.GetBytes(randomBytes);

        var refreshToken = new UserRefreshTokenDto
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenValidityInMinutes??1440),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            Timestamp = BitConverter.GetBytes(DateTime.UtcNow.ToBinary())
        };

        return refreshToken;
    }
}
