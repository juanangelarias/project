namespace CM.Model.Services;

public class MinimumUserResolverService : IUserResolverService
{
    public long? GetUserId()
    {
        throw new NotImplementedException();
    }

    public string GetUserName()
    {
        return "EntityFramework Migration";
    }
}