using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.UserRepositories
{
    public class UserRepositories : GenericRepositories<UserAccount>, IUserRepositories
    {
        public UserRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}