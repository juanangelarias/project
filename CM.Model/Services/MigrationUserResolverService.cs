namespace CM.Model.Services;

public class MigrationUserResolverService : IUserResolverService
{
    public long? GetIsrId() => null;

    public long? GetRsaId() => null;

    public long? GetUserId()
    {
        throw new System.NotImplementedException();
    }

    public string GetUserName()
    {
        return "EntityFramework Migration";
    }
}