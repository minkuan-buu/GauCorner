using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.UserTokenRepositories
{
    public class UserTokenRepositories : GenericRepositories<UserToken>, IUserTokenRepositories
    {
        public UserTokenRepositories(GauCornerContext context) : base(context)
        {
        }

        public async Task<UserToken> GetUserTokenByUserIdAsync(Guid userId)
        {
            return await Context.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<UserToken> GetUserTokenByRefreshTokenAsync(string refreshToken)
        {
            return await Context.UserTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }
        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken, Guid userId)
        {
            var userToken = await Context.UserTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.UserId == userId);
            return userToken != null;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, Guid userId)
        {
            var userToken = await Context.UserTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.UserId == userId);
            if (userToken != null)
            {
                Context.UserTokens.Remove(userToken);
                return true;
            }
            return false;
        }
    }
}