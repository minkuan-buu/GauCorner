using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GauCorner.Data.DTO.ResponseModel;

namespace GauCorner.Business.Services.ProductAttachmentService
{
    public interface IProductAttachmentServices
    {
        Task<ProductAttachmentStringModel> GetProductAttachmentById(Guid productId);
    }
}