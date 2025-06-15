using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.AttributeValueRepositories
{
    public class AttributeValueRepositories : GenericRepositories<AttributeValue>, IAttributeValueRepositories
    {
        public AttributeValueRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}