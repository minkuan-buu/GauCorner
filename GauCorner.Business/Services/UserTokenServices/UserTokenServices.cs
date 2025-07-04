using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.UserTokenRepositories;

namespace GauCorner.Business.Services.UserTokenServices;

public class UserTokenServices : IUserTokenServices
{
    private readonly IUserTokenRepositories _userTokenRepositories;
    public UserTokenServices(IUserTokenRepositories userTokenRepositories)
    {
        _userTokenRepositories = userTokenRepositories;
    }

    public async Task<UserToken> GetUserToken(Guid DeviceId)
    {
        return await _userTokenRepositories.GetSingle(x => x.Id == DeviceId);
    }

    public async Task UpdateUserToken(UserToken userToken)
    {
        await _userTokenRepositories.Update(userToken);
    }
}