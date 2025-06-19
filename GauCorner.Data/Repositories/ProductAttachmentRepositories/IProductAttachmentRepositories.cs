using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductAttachmentRepositories
{
    public interface IProductAttachmentRepositories : IGenericRepositories<ProductAttachment>
    {
        Task DeleteByProductId(Guid productId);
    }
}