using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductVariantRepositories
{
    public class ProductVariantRepositories : GenericRepositories<ProductVariant>, IProductVariantRepositories
    {
        public ProductVariantRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}