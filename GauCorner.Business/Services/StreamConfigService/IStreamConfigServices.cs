using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Business.Services.StreamConfigServices
{
    public interface IStreamConfigServices
    {
        // Define your methods here
        Task<ResultModel<ListDataResultModel<StreamConfigResModel>>> GetStreamConfig(string token);
        Task<StreamAttachmentResModel> GetStreamAttachment(string token);
        Task<ResultModel<MessageResultModel>> SaveStreamConfig(List<StreamConfigDto> streamConfigDtos, string token);
    }
}