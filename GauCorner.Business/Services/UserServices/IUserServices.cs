using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.DTO.ResponseModel.UserResModel;
using GauCorner.Data.Entities;

namespace GauCorner.Business.Services.UserServices
{
    public interface IUserServices
    {
        Task<ResultModel<UserLoginResModel>> Login(UserLoginModel userLoginModel);
        Task<ResultModel<MessageResultModel>> Register(UserRegisterModel userRegisterModel);
        Task<UserAccount> GetUserById(Guid id);
    }
}