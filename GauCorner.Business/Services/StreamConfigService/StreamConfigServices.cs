using System.Net;
using AutoMapper;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Enums.StreamConfigEnums;
using GauCorner.Data.Repositories.StreamConfigRepositories;

namespace GauCorner.Business.Services.StreamConfigServices
{
    public class StreamConfigServices : IStreamConfigServices
    {
        private readonly IStreamConfigRepositories _streamConfigRepositories;
        private readonly IMapper _mapper;

        public StreamConfigServices(IStreamConfigRepositories streamConfigRepositories, IMapper mapper)
        {
            _mapper = mapper;
            _streamConfigRepositories = streamConfigRepositories;
        }
        // Implement your methods here
        public async Task<ResultModel<ListDataResultModel<StreamConfigResModel>>> GetStreamConfig(string token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(token, "userid"));
            var streamConfigs = await _streamConfigRepositories.GetList(x => x.UserId == userId && x.StreamConfigType.Status == "Active", includeProperties: "StreamConfigType");
            List<StreamConfigResModel> streamConfigResModels = _mapper.Map<List<StreamConfigResModel>>(streamConfigs);
            return new ResultModel<ListDataResultModel<StreamConfigResModel>>
            {
                StatusCodes = (int)HttpStatusCode.OK,
                Response = new ListDataResultModel<StreamConfigResModel>
                {
                    Data = streamConfigResModels,
                }
            };
        }

        public async Task<StreamAttachmentResModel> GetStreamAttachment(string token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(token, "userid"));
            var streamConfigs = await _streamConfigRepositories.GetList(x => x.UserId == userId && x.StreamConfigType.Status == "Active", includeProperties: "StreamConfigType");
            if (streamConfigs != null && streamConfigs.Any())
            {
                var gif = streamConfigs.FirstOrDefault(x => x.StreamConfigType.AlternativeName == StreamConfigTypeEnums.Gif.ToString());
                var soundEffect = streamConfigs.FirstOrDefault(x => x.StreamConfigType.AlternativeName == StreamConfigTypeEnums.SoundEffect.ToString());
                return new StreamAttachmentResModel
                {
                    Gif = gif != null ? gif.Value : null,
                    SoundEffect = soundEffect != null ? soundEffect.Value : null,
                };
            }
            return null;
        }

        public async Task<ResultModel<MessageResultModel>> SaveStreamConfig(List<StreamConfigDto> streamConfigDtos, string token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(token, "userid"));

            foreach (var dto in streamConfigDtos)
            {
                // Tìm cấu hình hiện tại
                var existingConfig = await _streamConfigRepositories.GetSingle(
                    x => x.UserId == userId && x.Id == dto.Id && x.StreamConfigType.AlternativeName == dto.AlternativeName, includeProperties: "StreamConfigType"
                );

                if (existingConfig != null)
                {
                    existingConfig.Value = dto.Value;

                    await _streamConfigRepositories.Update(existingConfig);
                }

                // Nếu muốn insert mới khi chưa có, thì thêm else { ... }
            }

            return new ResultModel<MessageResultModel>
            {
                StatusCodes = (int)HttpStatusCode.OK,
                Response = new MessageResultModel
                {
                    Message = "Stream configuration updated successfully."
                }
            };
        }


    }
}