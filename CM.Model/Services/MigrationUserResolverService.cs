namespace CM.Model.Services;

public class MigrationUserResolverService : IUserResolverService
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