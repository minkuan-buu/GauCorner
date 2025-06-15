using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.VariantAttributeValueRepo
{
    public class VariantAttributeValueRepo : GenericRepositories<VariantAttributeValue>, IVariantAttributeValueRepo
    {
        public VariantAttributeValueRepo(GauCornerContext context) : base(context)
        {
        }
    }
}