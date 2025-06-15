using GauCorner.Data.Entities;

namespace GauCorner.Business.Services.UserTokenServices;

public interface IUserTokenServices
{
    Task<UserToken> GetUserToken(Guid DeviceId);
    Task UpdateUserToken(UserToken userToken);
}