using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.ProductVariantRepositories
{
    public class ProductVariantRepositories : GenericRepositories<ProductVariant>, IProductVariantRepositories
    {
        public ProductVariantRepositories(GauCornerContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(Guid productId)
        {
            var variants = await Context.ProductVariants
                .Where(v => v.ProductId == productId)
                .ToListAsync();

            if (variants.Any())
            {
                Context.ProductVariants.RemoveRange(variants);
                await Context.SaveChangesAsync();
            }
        }
    }
}