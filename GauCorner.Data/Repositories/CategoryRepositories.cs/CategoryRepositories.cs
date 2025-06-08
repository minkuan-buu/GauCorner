using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.GenericRepositories;

namespace GauCorner.Data.Repositories.CategoryRepositories
{
    public class CategoryRepositories : GenericRepositories<Category>, ICategoryRepositories
    {
        public CategoryRepositories(GauCornerContext context) : base(context)
        {
        }
    }
}