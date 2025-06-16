using AutoMapper;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Business.Utilities.Converter;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.AttributeValueRepositories;
using GauCorner.Data.Repositories.CategoryRepositories;
using GauCorner.Data.Repositories.ProductAttachmentRepositories;
using GauCorner.Data.Repositories.ProductAttributeRepositories;
using GauCorner.Data.Repositories.ProductRepositories;
using GauCorner.Data.Repositories.ProductVariantRepositories;
using GauCorner.Data.Repositories.VariantAttributeValueRepo;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using LinqKit;

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
        private readonly IProductAttachmentRepositories _productAttachmentRepositories;
        private readonly ICategoryRepositories _categoryRepositories;
        public ProductServices(IMapper mapper,
                               IProductRepositories productRepositories,
                               IProductAttributeRepositories productAttributeRepositories, IAttributeValueRepositories attributeValueRepositories, IProductVariantRepositories productVariantRepositories, IVariantAttributeValueRepo variantAttributeValueRepositories, IProductAttachmentRepositories productAttachmentRepositories, ICategoryRepositories categoryRepositories)
        {
            _mapper = mapper;
            _productRepositories = productRepositories;
            _productAttributeRepositories = productAttributeRepositories;
            _attributeValueRepositories = attributeValueRepositories;
            _productVariantRepositories = productVariantRepositories;
            _variantAttributeValueRepositories = variantAttributeValueRepositories;
            _productAttachmentRepositories = productAttachmentRepositories;
            _categoryRepositories = categoryRepositories;
        }
        public async Task<ResultModel<MessageResultModel>> CreateProduct(ProductDto productModel, string Token)
        {
            var productId = Guid.NewGuid();
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));

            var product = new Product
            {
                Id = productId,
                Name = TextConvert.ConvertToUnicodeEscape(productModel.Name),
                Description = TextConvert.ConvertToUnicodeEscape(productModel.Description),
                CategoryId = productModel.CategoryId,
                CreatedBy = userId,
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
                    Name = TextConvert.ConvertToUnicodeEscape(attr.Name),
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
                        Value = TextConvert.ConvertToUnicodeEscape(option.Value),
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

            var productImageUrls = new List<ProductAttachment>();
            // Assuming productModel.ProductImage is a list of image URLs or paths
            foreach (var image in productModel.ProductImage)
            {
                var attachment = new ProductAttachment
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Type = "Image",
                    AttachmentUrl = image, // Assuming image is the URL or path to the image
                    Index = productImageUrls.Count // Assign index based on current count
                };
                productImageUrls.Add(attachment);
            }

            await _productRepositories.Insert(product);
            await _productAttributeRepositories.InsertRange(productAttributes);
            await _attributeValueRepositories.InsertRange(attributeValues);
            await _productVariantRepositories.InsertRange(productVariants);
            await _variantAttributeValueRepositories.InsertRange(variantAttributeValues);
            await _productAttachmentRepositories.InsertRange(productImageUrls);
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

        public async Task<ResultModel<DataResultModel<ProductDetailDto>>> GetProductDetail(Guid productId, string slug)
        {
            var product = await _productRepositories.GetSingle(
                x => x.Id == productId && x.CreatedByNavigation.Path == slug,
                includeProperties: "CreatedByNavigation,Category,ProductAttachments,ProductAttributes.AttributeValues,ProductVariants.VariantAttributeValues.Value"
            );

            if (product == null)
                throw new Exception("Product not found");

            var attributeList = product.ProductAttributes
                .OrderByDescending(a => a.IsParent)
                .Select((attr, index) => new
                {
                    Attribute = attr,
                    Index = index,
                    OptionList = attr.AttributeValues.ToList()
                }).ToList();

            var categories = await _categoryRepositories.GetList();
            CategoryDto? categoryHierarchy = BuildCategoryHierarchy(categories.ToList(), product.CategoryId);
            var dto = new ProductDetailDto
            {
                Id = product.Id,
                Name = TextConvert.ConvertFromUnicodeEscape(product.Name),
                Description = TextConvert.ConvertFromUnicodeEscape(product.Description),
                Category = categoryHierarchy,
                MainCategoryName = TextConvert.ConvertFromUnicodeEscape(product.Category.Name),
                ProductImages = product.ProductAttachments.OrderBy(x => x.Index).Select(i => i.AttachmentUrl).ToList(),
                Attribute = attributeList.Select(a => new ProductAttributeDto
                {
                    Name = TextConvert.ConvertFromUnicodeEscape(a.Attribute.Name),
                    IsParent = a.Attribute.IsParent,
                    Options = a.OptionList.Select(opt => new ProductOptionDto
                    {
                        Id = opt.Id,
                        Value = TextConvert.ConvertFromUnicodeEscape(opt.Value),
                        Image = opt.Image
                    }).ToList()
                }).ToList(),
                Variant = product.ProductVariants.Select(variant =>
                {
                    // Mỗi attribute trong attributeList đại diện cho 1 nhóm giá trị (màu, size, etc.)
                    var attributeIndexList = attributeList.Select(attr =>
                    {
                        // Tìm valueId (là AttributeValue.Id) thuộc về Variant và trong nhóm này
                        var valueId = variant.VariantAttributeValues
                            .FirstOrDefault(vav => attr.OptionList.Any(o => o.Id == vav.ValueId))?.ValueId;

                        return valueId ?? Guid.Empty;
                    }).ToList();

                    return new ProductVariantDto
                    {
                        AttributeIndex = attributeIndexList,
                        Price = variant.Price,
                        Stock = variant.StockQuantity,
                        SKU = variant.Sku
                    };
                }).ToList()
            };
            return new ResultModel<DataResultModel<ProductDetailDto>>
            {
                StatusCodes = StatusCodes.Status200OK,
                Response = new DataResultModel<ProductDetailDto>
                {
                    Data = dto,
                }
            };
        }

        private CategoryDto BuildCategoryHierarchy(List<Category> allCategories, Guid leafCategoryId)
        {
            var path = new Stack<Category>();

            // Truy ngược từ leaf → root
            var current = allCategories.FirstOrDefault(c => c.Id == leafCategoryId);
            while (current != null)
            {
                path.Push(current);
                current = current.ParentCategoryId != null
                    ? allCategories.FirstOrDefault(c => c.Id == current.ParentCategoryId)
                    : null;
            }

            // Dựng cây CategoryDto từ root → leaf
            CategoryDto? root = null;
            CategoryDto? currentNode = null;

            while (path.Count > 0)
            {
                var cat = path.Pop();
                var node = new CategoryDto
                {
                    Id = cat.Id,
                    Name = TextConvert.ConvertFromUnicodeEscape(cat.Name),
                    Status = cat.Status,
                    SubCategories = new List<CategoryDto>()
                };

                if (root == null)
                {
                    root = node;
                    currentNode = node;
                }
                else
                {
                    currentNode!.SubCategories!.Add(node);
                    currentNode = node;
                }
            }

            return root!;
        }

        public async Task<ResultModel<ListDataResultModel<ProductResModel>>> GetAllProducts(PaginationRequest request, string userPath)
        {
            // B1: Tạo filter cơ bản
            Expression<Func<Product, bool>> filter = p => true;

            // Chỉ thêm các filter có thể dịch sang SQL
            filter = filter.And(p => p.CreatedByNavigation.Path == userPath);

            if (request.CategoryId.HasValue)
            {
                filter = filter.And(p => p.CategoryId == request.CategoryId);
            }

            // B2: Chọn kiểu sắp xếp
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy;

            if (request.SortBy == "price")
            {
                orderBy = (request.IsDescending ?? false)
                    ? q => q.OrderByDescending(p => p.ProductVariants.Max(v => v.Price))
                    : q => q.OrderBy(p => p.ProductVariants.Min(v => v.Price));
            }
            else
            {
                orderBy = (request.IsDescending ?? false)
                    ? q => q.OrderByDescending(p => p.Name)
                    : q => q.OrderBy(p => p.Name);
            }

            // B3: Lấy dữ liệu trước, sau đó lọc keyword trên client
            var pagedData = await _productRepositories.GetPagedList(
                filter: filter,
                orderBy: orderBy,
                includeProperties: "ProductVariants,ProductAttachments,CreatedByNavigation",
                pageIndex: request.Page ?? 1,
                pageSize: request.PageSize ?? 8
            );

            var data = pagedData.Data;

            // B4: Lọc keyword sau khi đã lấy dữ liệu về (client-side)
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.ToLower();
                data = data.Where(p => TextConvert.ConvertFromUnicodeEscape(p.Name).ToLower().Contains(keyword)
                ).ToList();
            }

            // B5: Map kết quả
            var response = new ListDataResultModel<ProductResModel>
            {
                CurrentPage = pagedData.CurrentPage,
                PageSize = pagedData.PageSize,
                TotalItems = pagedData.TotalItems,
                TotalPages = pagedData.TotalPages,
                Data = data.Select(p => new ProductResModel
                {
                    Id = p.Id,
                    Name = TextConvert.ConvertFromUnicodeEscape(p.Name),
                    Thumbnail = p.ProductAttachments.OrderBy(x => x.Index).FirstOrDefault()?.AttachmentUrl ?? "",
                    Variants = (
                        request.SortBy == "price" && (request.IsDescending ?? false)
                            ? p.ProductVariants.OrderByDescending(v => v.Price)
                            : p.ProductVariants.OrderBy(v => v.Price)
                    )
                    .Select(v => new ProductVariantAllDto
                    {
                        Price = v.Price,
                        Stock = v.StockQuantity,
                        SKU = v.Sku
                    }).ToList()
                }).ToList()
            };

            return new ResultModel<ListDataResultModel<ProductResModel>>
            {
                StatusCodes = StatusCodes.Status200OK,
                Response = response
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