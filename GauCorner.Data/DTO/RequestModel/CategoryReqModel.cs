using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GauCorner.Data.DTO.RequestModel
{
    public class CategoryReqModel
    {
        public string CategoryName { get; set; } = null!;
        public List<SubCategoryReqModel>? SubCategories { get; set; }
    }

    public class SubCategoryReqModel
    {
        public string CategoryName { get; set; } = null!;
    }

    public class CategoryUpdateReqModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? ParentCategoryId { get; set; }
    }
}
