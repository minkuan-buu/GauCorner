using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GauCorner.Services.CategoryServices
{
    public interface ICategoryServices
    {
        public Task<List<CategoryResModel>> GetListCategory();
        public Task<MessageResultModel> CreateCategory(CategoryReqModel categoryReqModel);
        //public Task<MessageResultModel> UpdateCategory(CategoryUpdateReqModel categoryUpdateReqModel);
    }
}
