using System.Net;
using AutoMapper;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Data.DTO.Custom;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.DTO.ResponseModel.UserResModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Repositories.StreamConfigRepositories;
using GauCorner.Data.Repositories.StreamConfigTypeRepositories;
using GauCorner.Data.Repositories.UserRepositories;
using GauCorner.Data.Repositories.UserTokenRepositories;
using Microsoft.VisualBasic;

namespace GauCorner.Business.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepositories _userRepositories;
        private readonly IStreamConfigTypeRepositories _streamConfigTypeRepositories;
        private readonly IStreamConfigRepositories _streamConfigRepositories;
        private readonly IUserTokenRepositories _userTokenRepositories;
        private readonly IMapper _mapper;

        public UserServices(IUserRepositories userRepositories, IMapper mapper, IStreamConfigTypeRepositories streamConfigTypeRepositories, IStreamConfigRepositories streamConfigRepositories, IUserTokenRepositories userTokenRepositories)
        {
            _streamConfigTypeRepositories = streamConfigTypeRepositories;
            _streamConfigRepositories = streamConfigRepositories;
            _userRepositories = userRepositories;
            _userTokenRepositories = userTokenRepositories;
            _mapper = mapper;
        }

        public async Task<ResultModel<DataResultModel<UserLoginResModel>>> Login(UserLoginModel userLoginModel)
        {
            try
            {
                var user = await _userRepositories.GetSingle(x => x.Username == userLoginModel.Username && x.Status == "Active");
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (!Authentication.VerifyPasswordHashed(userLoginModel.Password, user.Salt, user.Password))
                {
                    throw new Exception("Password is incorrect");
                }
                var Result = _mapper.Map<UserLoginResModel>(user);
                UserToken NewUserToken = new UserToken()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    AccesToken = Result.Auth.Token,
                    RefreshToken = Result.Auth.RefreshToken,
                    CreatedAt = DateTime.Now
                };
                Result.Auth.DeviceId = NewUserToken.Id;
                await _userTokenRepositories.Insert(NewUserToken);
                return new ResultModel<DataResultModel<UserLoginResModel>>()
                {
                    StatusCodes = (int)HttpStatusCode.OK,
                    Response = new DataResultModel<UserLoginResModel>()
                    {
                        Data = Result
                    }
                };
            }
            catch (Exception ex)
            {
                throw new CustomException($"Login failed: {ex.Message}");
            }

        }

        public async Task<ResultModel<MessageResultModel>> Register(UserRegisterModel userRegisterModel)
        {
            try
            {
                var existingUser = await _userRepositories.GetSingle(x => x.Username == userRegisterModel.Username && x.Status == "Active");
                if (existingUser != null)
                {
                    throw new Exception("Username already exists");
                }
                var newUser = _mapper.Map<UserAccount>(userRegisterModel);
                CreateHashPasswordModel newPassword = Authentication.CreateHashPassword(userRegisterModel.Password);
                newUser.Password = newPassword.HashedPassword;
                newUser.Salt = newPassword.Salt;
                await _userRepositories.Insert(newUser);
                var StreamConfigType = await _streamConfigTypeRepositories.GetList(x => x.Status == "Active");
                if (StreamConfigType == null || StreamConfigType.ToList().Count == 0)
                {
                    throw new Exception("StreamConfigType is not configured");
                }
                List<StreamConfig> streamConfigs = new List<StreamConfig>();
                foreach (var item in StreamConfigType)
                {
                    var streamConfig = new StreamConfig()
                    {
                        Id = Guid.NewGuid(),
                        UserId = newUser.Id,
                        StreamConfigTypeId = item.Id,
                        Value = item.DefaultValue,
                    };
                    streamConfigs.Add(streamConfig);
                }
                await _streamConfigRepositories.InsertRange(streamConfigs);
                return new ResultModel<MessageResultModel>()
                {
                    StatusCodes = 200,
                    Response = new MessageResultModel()
                    {
                        Message = "Ok",
                    }
                };
            }
            catch (Exception ex)
            {
                throw new CustomException($"Register failed: {ex.Message}");
            }
        }

        public async Task<ResultModel<MessageResultModel>> Logout(Guid DeviceId)
        {
            var MessageReturn = "Logout success! ";
            var UserToken = await _userTokenRepositories.GetSingle(x => x.Id == DeviceId);
            if (UserToken == null)
            {
                MessageReturn += "Warning: DeviceId is not found!";
            }
            else await _userTokenRepositories.Delete(UserToken);
            return new ResultModel<MessageResultModel>()
            {
                StatusCodes = (int)HttpStatusCode.OK,
                Response = new MessageResultModel()
                {
                    Message = MessageReturn
                }
            };
        }

        public async Task<UserAccount> GetUserById(Guid id)
        {
            return await _userRepositories.GetSingle(x => x.Id == id);
        }
    }
}