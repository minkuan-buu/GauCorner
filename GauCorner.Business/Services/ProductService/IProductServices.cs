using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Business.Services.ProductServices
{
    public interface IProductServices
    {
        // Task<ResultModel<ProductPageResModel>> GetProductPage(string userPath);
        // Task<ResultModel<ProductDetailResModel>> GetProductDetail(int productId, string userPath);
        Task<ResultModel<MessageResultModel>> CreateProduct(ProductDto productModel, string Token);
    }
}