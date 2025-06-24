using System.Net;
using AutoMapper;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
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
    }
}