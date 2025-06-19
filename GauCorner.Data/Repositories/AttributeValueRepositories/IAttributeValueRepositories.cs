using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.AttributeValueRepositories
{
    public interface IAttributeValueRepositories : IGenericRepositories<AttributeValue>
    {
        Task DeleteByProductId(Guid productId);
    }
}