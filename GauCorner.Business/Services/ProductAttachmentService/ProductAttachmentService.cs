using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.Repositories.ProductAttachmentRepositories;
using GauCorner.Data.Repositories.ProductAttributeRepositories;
namespace GauCorner.Business.Services.ProductAttachmentService
{

    public class ProductAttachmentServices : IProductAttachmentServices
    {
        private readonly IProductAttributeRepositories _productAttachmentRepository;
        private readonly IProductAttachmentRepositories _productAttachmentRepositories;

        public ProductAttachmentServices(IProductAttributeRepositories productAttachmentRepository,
            IProductAttachmentRepositories productAttachmentRepositories)
        {
            _productAttachmentRepository = productAttachmentRepository;
            _productAttachmentRepositories = productAttachmentRepositories;
        }

        public async Task<ProductAttachmentStringModel> GetProductAttachmentById(Guid productId)
        {
            var productAttributes = await _productAttachmentRepository.GetList(x => x.ProductId == productId, includeProperties: "AttributeValues");
            var productImages = await _productAttachmentRepositories.GetList(x => x.ProductId == productId && x.Type == "Image");

            var result = new ProductAttachmentStringModel
            {
                ProductImages = productImages.Select(x => x.AttachmentUrl).ToList(),
                AttributeImages = new List<string>()
            };

            foreach (var attribute in productAttributes.ToList().SelectMany(x => x.AttributeValues))
            {
                if (!string.IsNullOrEmpty(attribute.Image))
                {
                    result.AttributeImages.Add(attribute.Image);
                }
            }

            return result;
        }
    }
}