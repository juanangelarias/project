namespace CM.Model.Services;

public interface IUserResolverService
{
    string GetUserName();
    long? GetUserId();
}