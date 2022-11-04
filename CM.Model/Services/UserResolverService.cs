using Microsoft.AspNetCore.Http;

namespace CM.Model.Services;

public class UserResolverService: IUserResolverService
{
    private readonly IHttpContextAccessor _context;
    private string _userName;
    private long? _userId;

    public UserResolverService(IHttpContextAccessor context)
    {
        _context = context;
        SetUserName();
        SetUserId();
    }

    private void SetUserName()
    {
        // Unknown should never be hit, but if we have an unauthenticated API End Point that can persist data
        // the Idenitty will be empty.  Use "Unknown" as a placeholder value.
        var user = _context.HttpContext?.User;
        string userName = "Unknown";
        if (user != null && user.Claims?.Count() > 0)
        {
            userName = GetClaimValue("UserName");
        }

        _userName = userName;
    }

    private void SetUserId()
    {
        // Unknown should never be hit, but if we have an unauthenticated API End Point that can persist data
        // the Idenitty will be empty.  Use "" as a placeholder value.
        var user = _context.HttpContext?.User;
        if (user != null && user.Claims?.Count() > 0)
        {
            _userId = Convert.ToInt64(GetClaimValue("Id"));
        }
    }

    protected string GetClaimValue(string claim)
    {
        return _context.HttpContext?.User?.Claims.FirstOrDefault(p => p.Type == claim)?.Value;
    }

    public string GetUserName() => _userName;
    public long? GetUserId() => _userId;



}