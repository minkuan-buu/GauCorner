using AutoMapper;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.AttributeValueRepositories;
using GauCorner.Data.Repositories.ProductAttributeRepositories;
using GauCorner.Data.Repositories.ProductRepositories;
using GauCorner.Data.Repositories.ProductVariantRepositories;
using GauCorner.Data.Repositories.VariantAttributeValueRepo;
using Microsoft.AspNetCore.Http;

namespace GauCorner.Business.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly IMapper _mapper;
        private readonly IProductRepositories _productRepositories;
        private readonly IProductAttributeRepositories _productAttributeRepositories;
        private readonly IAttributeValueRepositories _attributeValueRepositories;
        private readonly IProductVariantRepositories _productVariantRepositories;
        private readonly IVariantAttributeValueRepo _variantAttributeValueRepositories;
        public ProductServices(IMapper mapper,
                               IProductRepositories productRepositories,
                               IProductAttributeRepositories productAttributeRepositories, IAttributeValueRepositories attributeValueRepositories, IProductVariantRepositories productVariantRepositories, IVariantAttributeValueRepo variantAttributeValueRepositories)
        {
            _mapper = mapper;
            _productRepositories = productRepositories;
            _productAttributeRepositories = productAttributeRepositories;
            _attributeValueRepositories = attributeValueRepositories;
            _productVariantRepositories = productVariantRepositories;
            _variantAttributeValueRepositories = variantAttributeValueRepositories;
        }
        public async Task<ResultModel<MessageResultModel>> CreateProduct(ProductDto productModel)
        {
            var productId = Guid.NewGuid();

            var product = new Product
            {
                Id = productId,
                Name = productModel.Name,
                Description = productModel.Description,
                CategoryId = productModel.CategoryId,
                Status = true
            };

            var productAttributes = new List<ProductAttribute>();
            var attributeValues = new List<AttributeValue>();
            var attributeIdMap = new List<(int attrIndex, Guid attrId)>();

            for (int attrIndex = 0; attrIndex < productModel.Attribute.Count; attrIndex++)
            {
                var attr = productModel.Attribute[attrIndex];
                var attrId = Guid.NewGuid();

                productAttributes.Add(new ProductAttribute
                {
                    Id = attrId,
                    ProductId = productId,
                    Name = attr.Name,
                    IsParent = attr.isParent,
                });

                attributeIdMap.Add((attrIndex, attrId));

                for (int optIndex = 0; optIndex < attr.Options.Count; optIndex++)
                {
                    var option = attr.Options[optIndex];

                    if (string.IsNullOrWhiteSpace(option.Value)) continue;

                    attributeValues.Add(new AttributeValue
                    {
                        Id = Guid.NewGuid(),
                        AttributeId = attrId,
                        Value = option.Value,
                        Image = option.Image
                    });
                }
            }

            var productVariants = new List<ProductVariant>();
            var variantAttributeValues = new List<VariantAttributeValue>();

            foreach (var variant in productModel.Variant)
            {
                var variantId = Guid.NewGuid();

                productVariants.Add(new ProductVariant
                {
                    Id = variantId,
                    ProductId = productId,
                    Sku = variant.SKU,
                    Price = variant.Price,
                    StockQuantity = variant.Stock
                });

                for (int i = 0; i < variant.AttributeIndex.Count; i++)
                {
                    var attrIdx = i;
                    var optIdx = variant.AttributeIndex[i];

                    var attrId = attributeIdMap.First(x => x.attrIndex == attrIdx).attrId;
                    var values = attributeValues.Where(v => v.AttributeId == attrId).ToList();

                    if (optIdx >= 0 && optIdx < values.Count)
                    {
                        var valueId = values[optIdx].Id;

                        variantAttributeValues.Add(new VariantAttributeValue
                        {
                            VariantId = variantId,
                            ValueId = valueId
                        });
                    }
                }
            }

            await _productRepositories.Insert(product);
            await _productAttributeRepositories.InsertRange(productAttributes);
            await _attributeValueRepositories.InsertRange(attributeValues);
            await _productVariantRepositories.InsertRange(productVariants);
            await _variantAttributeValueRepositories.InsertRange(variantAttributeValues);
            // Save all to database
            // Assuming you use Entity Framework DbContext
            // Example:
            //_dbContext.Products.Add(product);
            //_dbContext.ProductAttributes.AddRange(productAttributes);
            //_dbContext.AttributeValues.AddRange(attributeValues);
            //_dbContext.ProductVariants.AddRange(productVariants);
            //_dbContext.VariantAttributeValues.AddRange(variantAttributeValues);
            //await _dbContext.SaveChangesAsync();

            return new ResultModel<MessageResultModel>
            {
                StatusCodes = StatusCodes.Status200OK,
                Response = new MessageResultModel
                {
                    Message = "Product created successfully",
                }
            };
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