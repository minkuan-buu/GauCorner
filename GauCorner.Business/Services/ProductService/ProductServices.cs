using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Business.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        public Task<ResultModel<MessageResultModel>> CreateProduct(ProductFormReqModel productModel, List<ProductVariant> productVariants)
        {
            // Implementation for creating a product
            throw new NotImplementedException();
        }

        public Task<ResultModel<MessageResultModel>> DeleteProduct(int productId, string userPath)
        {
            // Implementation for deleting a product
            throw new NotImplementedException();
        }

        public Task<ResultModel<MessageResultModel>> UpdateProduct(ProductReqModel productModel, string userPath)
        {
            // Implementation for updating a product
            throw new NotImplementedException();
        }

        // Uncomment and implement these methods if needed
        /*
        public Task<ResultModel<ProductPageResModel>> GetProductPage(string userPath)
        {
            // Implementation for getting product page
            throw new NotImplementedException();
        }

        public Task<ResultModel<ProductDetailResModel>> GetProductDetail(int productId, string userPath)
        {
            // Implementation for getting product detail
            throw new NotImplementedException();
        }
        */
    }
}