namespace CM.Model.Services;

public class MigrationUserResolverService : IUserResolverService
{
    public long? GetUserId()
    {
        return -1;
    }

    public string GetUserName()
    {
        return "EntityFramework Migration";
    }
}