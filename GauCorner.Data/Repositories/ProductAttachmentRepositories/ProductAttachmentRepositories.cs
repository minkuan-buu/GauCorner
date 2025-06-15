using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductAttachmentRepositories
{
    public class ProductAttachmentRepositories : GenericRepositories<ProductAttachment>, IProductAttachmentRepositories
    {
        public ProductAttachmentRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}