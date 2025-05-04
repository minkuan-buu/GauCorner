using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.DonateRepositories
{
    public class DonateRepositories : GenericRepositories<Donate>, IDonateRepositories
    {
        public DonateRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}