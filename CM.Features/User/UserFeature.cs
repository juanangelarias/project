using CM.Model.General;
using CM.Repositories;

namespace CM.Features;

public class UserFeature : IUserFeature
{
    private readonly IUserRepository _userRepository;

    public UserFeature(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<bool> Login(LoginData login)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetPassword(ResetPassword data)
    {
        throw new NotImplementedException();
    }
}