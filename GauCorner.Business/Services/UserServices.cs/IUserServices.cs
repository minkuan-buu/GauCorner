using GauCorne.Data.DTO.RequestModel;
using GauCorne.Data.DTO.ResponseModel.UserResModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Business.Services.UserServices
{
    public interface IUserServices
    {
        Task<ResultModel<UserLoginResModel>> Login(UserLoginModel userLoginModel);
        Task<ResultModel<MessageResultModel>> Register(UserRegisterModel userRegisterModel);
    }
}