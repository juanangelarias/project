namespace CM.Model.Services;

public class MinimumUserResolverService : IUserResolverService
{
    public long? GetUserId()
    {
        return -1;
    }

    public string GetUserName()
    {
        return "System User";
    }
}