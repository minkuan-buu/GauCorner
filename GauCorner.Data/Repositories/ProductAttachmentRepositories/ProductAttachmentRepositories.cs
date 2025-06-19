using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GauCorner.Data.Repositories.ProductAttachmentRepositories
{
    public class ProductAttachmentRepositories : GenericRepositories<ProductAttachment>, IProductAttachmentRepositories
    {
        public ProductAttachmentRepositories(GauCornerContext context) : base(context)
        {
        }

        public async Task DeleteByProductId(Guid productId)
        {
            var attachments = await Context.ProductAttachments
                .Where(pa => pa.ProductId == productId)
                .ToListAsync();

            if (attachments.Any())
            {
                Context.ProductAttachments.RemoveRange(attachments);
                await Context.SaveChangesAsync();
            }
        }
    }
}