using CM.Core.Authentication;
using CM.Repositories;
using Microsoft.AspNetCore.Http;

namespace CM.Core.Middleware;

public class JwtMiddleware: IMiddleware
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IUserRepository _userRepository;

    public JwtMiddleware(IJwtGenerator jwtGenerator, IUserRepository userRepository)
    {
        _jwtGenerator = jwtGenerator;
        _userRepository = userRepository;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context
            .Request
            .Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        if (token != null)
        {
            await AttachUserToContext(context, token);
        }

        await next.Invoke(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var claimPrincipal = _jwtGenerator.ValidateToken(token);
            if (claimPrincipal != null)
            {
                var userId = Convert.ToInt64(claimPrincipal.Claims.First(x => x.Type == "Id").Value);
                context.Items["User"] = userId != 0
                    ? await _userRepository.GetByIdExpandedAsync(userId)
                    : null;
            }
        }
        catch
        {
            // ignored
        }
    }
}