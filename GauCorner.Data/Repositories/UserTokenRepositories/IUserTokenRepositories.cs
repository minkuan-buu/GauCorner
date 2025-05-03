using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.UserTokenRepositories
{
    public interface IUserTokenRepositories : IGenericRepositories<UserToken>
    {
        Task<UserToken> GetUserTokenByUserIdAsync(Guid userId);
        Task<UserToken> GetUserTokenByRefreshTokenAsync(string refreshToken);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken, Guid userId);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken, Guid userId);
    }
}