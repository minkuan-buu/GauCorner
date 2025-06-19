using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.AttributeValueRepositories
{
    public class AttributeValueRepositories : GenericRepositories<AttributeValue>, IAttributeValueRepositories
    {
        public AttributeValueRepositories(GauCornerContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(Guid productId)
        {
            var attributeValues = await Context.AttributeValues
                .Where(av => av.Variants.Any(v => v.ProductId == productId))
                .ToListAsync();

            if (attributeValues != null && attributeValues.Any())
            {
                Context.AttributeValues.RemoveRange(attributeValues);
                await Context.SaveChangesAsync();
            }
        }
    }
}