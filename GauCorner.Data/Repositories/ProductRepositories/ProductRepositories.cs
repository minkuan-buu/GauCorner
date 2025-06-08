using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductRepositories
{
    public class ProductRepositories : GenericRepositories<Product>, IProductRepositories
    {
        public ProductRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}