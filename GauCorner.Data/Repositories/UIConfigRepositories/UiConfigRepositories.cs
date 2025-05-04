using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.UIConfigRepositories
{

    public class UIConfigRepositories : GenericRepositories<Uiconfig>, IUIConfigRepositories
    {
        public UIConfigRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}