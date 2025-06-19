using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.ProductAttributeRepositories
{
    public interface IProductAttributeRepositories : IGenericRepositories<ProductAttribute>
    {
        Task DeleteByProductId(Guid productId);
    }
}