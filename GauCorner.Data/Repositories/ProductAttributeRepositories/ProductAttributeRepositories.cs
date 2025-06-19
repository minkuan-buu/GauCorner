using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.ProductAttributeRepositories
{
    public class ProductAttributeRepositories : GenericRepositories<ProductAttribute>, IProductAttributeRepositories
    {
        public ProductAttributeRepositories(GauCornerContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(Guid productId)
        {
            var productAttribute = await Context.ProductAttributes.Where(pa => pa.ProductId == productId).ToListAsync();
            if (productAttribute != null && productAttribute.Any())
            {
                Context.ProductAttributes.RemoveRange(productAttribute);
                await Context.SaveChangesAsync();
            }
        }
    }
}