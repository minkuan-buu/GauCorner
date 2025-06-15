using AutoMapper;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.Custom;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Enums;
using GauCorner.Data.Repositories.CategoryRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GauCorner.Business.Utilities.Converter;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Services.CategoryServices
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepositories _categoryRepositories;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRepositories categoryRepositories, IMapper mapper)
        {
            _categoryRepositories = categoryRepositories;
            _mapper = mapper;
        }

        public async Task<List<CategoryResModel>> GetListCategory()
        {
            var AllCategory = await _categoryRepositories.GetList();
            var ParentCategory = AllCategory.Where(x => x.ParentCategoryId == null).ToList();
            List<CategoryResModel> CategoryRes = _mapper.Map<List<CategoryResModel>>(ParentCategory);
            var ChildCategory = AllCategory.Where(x => x.ParentCategoryId != null).ToList();
            foreach (var parentCategory in CategoryRes)
            {
                var subCategory = ChildCategory.Where(x => x.ParentCategoryId.Equals(parentCategory.Id)).ToList();
                List<SubCategoryResModel> subCategoryRes = _mapper.Map<List<SubCategoryResModel>>(subCategory);
                parentCategory.SubCategories = subCategoryRes;
            }
            return CategoryRes;
        }

        public async Task<MessageResultModel> CreateCategory(CategoryReqModel categoryReqModel)
        {
            Guid NewId = Guid.NewGuid();
            var CheckExist = await _categoryRepositories.GetSingle(x => x.Name.Equals(TextConvert.ConvertToUnicodeEscape(categoryReqModel.CategoryName)));
            if (CheckExist != null)
            {
                throw new CustomException("Parent Category Name is Exist");
            }
            Category NewCategory = _mapper.Map<Category>(categoryReqModel);
            NewCategory.Id = NewId;
            NewCategory.Status = true;
            List<Category> SubCategories = _mapper.Map<List<Category>>(categoryReqModel.SubCategories);
            foreach (var item in SubCategories)
            {
                item.Id = Guid.NewGuid();
                item.ParentCategoryId = NewId;
                item.Status = true;
            }
            await _categoryRepositories.Insert(NewCategory);
            await _categoryRepositories.InsertRange(SubCategories);
            return new MessageResultModel()
            {
                Message = "Ok",
            };
        }

        //public async Task<MessageResultModel> UpdateCategory(CategoryUpdateReqModel categoryUpdateReqModel)
        //{

        //}
    }
}
