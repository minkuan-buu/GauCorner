using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductAttributeRepositories
{
    public class ProductAttributeRepositories : GenericRepositories<ProductAttribute>, IProductAttributeRepositories
    {
        public ProductAttributeRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}