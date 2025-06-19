using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.VariantAttributeValueRepo
{
    public class VariantAttributeValueRepo : GenericRepositories<VariantAttributeValue>, IVariantAttributeValueRepo
    {
        public VariantAttributeValueRepo(GauCornerContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(Guid productId)
        {

            var variantAttributeValues = await Context.ProductVariants
                .Where(v => v.ProductId == productId)
                .SelectMany(v => v.VariantAttributeValues)
                .ToListAsync();
            if (variantAttributeValues != null && variantAttributeValues.Any())
            {
                Context.VariantAttributeValues.RemoveRange(variantAttributeValues);
                await Context.SaveChangesAsync();
            }
        }
    }
}