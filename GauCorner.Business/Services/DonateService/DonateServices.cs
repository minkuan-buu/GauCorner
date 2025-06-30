using AutoMapper;
using GauCorner.Business.Services.UserServices;
using GauCorner.Business.Services.ZaloPay.Helper;
using GauCorner.Business.Services.ZaloPay.Helper.Crypto;
using GauCorner.Business.Utilities.Authentication;
using GauCorner.Data.DTO.Custom;
using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;
using GauCorner.Data.Entities;
using GauCorner.Data.Enums.TransactionEnums;
using GauCorner.Data.Repositories.DonateRepositories;
using GauCorner.Data.Repositories.UIConfigRepositories;
using GauCorner.Data.Repositories.UserRepositories;
using Newtonsoft.Json;

namespace GauCorner.Business.Services.DonateServices
{
    public class DonateServices : IDonateServices
    {
        private readonly IDonateRepositories _donateRepositories;
        private readonly IUserRepositories _userRepositories;
        private readonly IUIConfigRepositories _uiConfigRepositories;
        private readonly IMapper _mapper;
        private readonly string app_id = Environment.GetEnvironmentVariable("ZPAppId");
        private readonly string key1 = Environment.GetEnvironmentVariable("ZPKey1");
        private readonly string create_order_url = "https://sb-openapi.zalopay.vn/v2/create";
        private readonly string query_order_url = "https://sb-openapi.zalopay.vn/v2/query";
        private readonly string callback_url = Environment.GetEnvironmentVariable("CALLBACK_URL");

        public DonateServices(IDonateRepositories donateRepositories, IMapper mapper, IUserRepositories userRepositories,
            IUIConfigRepositories uiConfigRepositories)
        {
            _donateRepositories = donateRepositories;
            _uiConfigRepositories = uiConfigRepositories;
            _mapper = mapper;
            _userRepositories = userRepositories;
        }

        public async Task<ResultModel<MessageResultModel>> CreateDonate(DonateReqModel donateModel, string userPath)
        {
            try
            {
                var newDonate = _mapper.Map<Donate>(donateModel);
                var checkStreamer = await _userRepositories.GetSingle(x => x.Path == userPath);
                if (checkStreamer == null)
                {
                    throw new Exception("Người nhận không tồn tại trong hệ thống!");
                }
                Random rnd = new Random();
                var app_trans_id = DateTime.Now.ToString("yyMMdd") + "_" + rnd.Next(1000000); // Generate a random order's ID.
                var embed_data = new { promotioninfo = "GauCorner", app_user = checkStreamer.Id, app_time = Utils.GetTimeStamp(), amount = donateModel.Amount, description = "GauCorner - Thanh toán đơn hàng #" + app_trans_id, redirecturl = callback_url };
                var items = new[] { new { } };
                var param = new Dictionary<string, string>();
                newDonate.TransId = app_trans_id;
                newDonate.UserId = checkStreamer.Id;
                newDonate.PaymentStatus = TransactionStatus.PENDING.ToString();
                await _donateRepositories.Insert(newDonate);
                param.Add("app_id", app_id);
                param.Add("app_user", "user123");
                param.Add("app_time", Utils.GetTimeStamp().ToString());
                param.Add("amount", ((long)donateModel.Amount).ToString());
                param.Add("app_trans_id", app_trans_id); // mã giao dich có định dạng yyMMdd_xxxx
                param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
                param.Add("item", JsonConvert.SerializeObject(items));
                param.Add("description", "GauCorner - Thanh toán đơn hàng #" + app_trans_id);
                param.Add("callback_url", callback_url); // địa chỉ callback của bạn
                //param.Add("bank_code", "zalopayapp");

                var data = app_id + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                    + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
                param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

                var result = await HttpHelper.PostFormAsync(create_order_url, param);

                foreach (var entry in result)
                {
                    Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
                }
                var order_url = result["order_url"].ToString();
                return new ResultModel<MessageResultModel>
                {
                    StatusCodes = 200,
                    Response = new MessageResultModel
                    {
                        Message = order_url,
                    }
                };
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }

        public async Task<ResultModel<DonatePageResModel>> GetDonatePage(string userPath)
        {
            var getDonatePage = await _userRepositories.GetSingle(x => x.Path == userPath && x.Uiconfigs.FirstOrDefault(u => u.IsUsed) != null, includeProperties: "Uiconfigs");
            if (getDonatePage == null)
            {
                throw new CustomException("Người nhận không tồn tại trong hệ thống!");
            }
            var donatePage = _mapper.Map<DonatePageResModel>(getDonatePage);
            return new ResultModel<DonatePageResModel>
            {
                StatusCodes = 200,
                Response = donatePage
            };
        }

        public async Task<ResultModel<TransactionStatusResModel>> HandleCheckTransaction(string apptransid)
        {
            var param = new Dictionary<string, string>();
            param.Add("app_id", app_id);
            param.Add("app_trans_id", apptransid);
            var data = app_id + "|" + apptransid + "|" + key1;

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

            var result = await HttpHelper.PostFormAsync(query_order_url, param);

            foreach (var entry in result)
            {
                Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
            }
            var return_code = result["return_code"].ToString();
            var message = result["return_message"].ToString();
            var is_processing = result["is_processing"];
            var amount = result["amount"].ToString();

            var checkTransaction = await _donateRepositories.GetSingle(x => x.TransId == apptransid);
            if (int.TryParse(return_code, out int returnCode) && returnCode == 1)
            {
                if (checkTransaction != null)
                {
                    checkTransaction.PaymentStatus = TransactionStatus.SUCCESS.ToString();
                    await _donateRepositories.Update(checkTransaction);
                }
            }
            else if (int.TryParse(return_code, out returnCode) && returnCode == 2)
            {
                if (checkTransaction != null)
                {
                    checkTransaction.PaymentStatus = TransactionStatus.FAIL.ToString();
                    await _donateRepositories.Update(checkTransaction);
                }
            }
            else if (int.TryParse(return_code, out returnCode) && returnCode == 3)
            {
                if (checkTransaction != null)
                {
                    checkTransaction.PaymentStatus = TransactionStatus.PROCESSING.ToString();
                    await _donateRepositories.Update(checkTransaction);
                }
            }

            return new ResultModel<TransactionStatusResModel>
            {
                StatusCodes = 200,
                Response = new TransactionStatusResModel
                {
                    ReturnCode = int.Parse(return_code),
                    PaymentStatus = message,
                    Amount = checkTransaction.Amount,
                    isProcessing = is_processing != null ? bool.Parse(is_processing.ToString()) : false
                }
            };
        }

        public async Task<ResultModel<ListDataResultModel<DonatePageConfigLabel>>> GetConfigLabel(string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUser = await _userRepositories.GetList(x => x.Id == userId);
            if (getUser == null || !getUser.Any())
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var donatePageConfigs = await _uiConfigRepositories.GetList(x => x.CreatedBy == userId);
            var result = _mapper.Map<List<DonatePageConfigLabel>>(donatePageConfigs);
            return new ResultModel<ListDataResultModel<DonatePageConfigLabel>>
            {
                StatusCodes = 200,
                Response = new ListDataResultModel<DonatePageConfigLabel>
                {
                    Data = result
                }
            };
        }

        public async Task<ResultModel<DataResultModel<DonatePageConfigResModel>>> GetConfigById(Guid configId, string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUserAccount = await _userRepositories.GetSingle(x => x.Id == userId);
            if (getUserAccount == null)
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var GetConfig = await _uiConfigRepositories.GetSingle(x => x.Id == configId && x.CreatedBy == userId);
            if (GetConfig == null)
            {
                throw new CustomException("Cấu hình không tồn tại trong hệ thống!");
            }
            var result = _mapper.Map<DonatePageConfigResModel>(GetConfig);
            return new ResultModel<DataResultModel<DonatePageConfigResModel>>
            {
                StatusCodes = 200,
                Response = new DataResultModel<DonatePageConfigResModel>
                {
                    Data = result
                }
            };
        }

        public async Task<ResultModel<MessageResultModel>> CreateConfig(ConfigDto request, string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUserAccount = await _userRepositories.GetSingle(x => x.Id == userId);
            if (getUserAccount == null)
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var existingConfig = await _uiConfigRepositories.GetSingle(x => x.CreatedBy == userId, orderBy: x => x.OrderByDescending(y => y.Index));
            var newConfig = _mapper.Map<Uiconfig>(request);
            newConfig.CreatedBy = userId;
            newConfig.IsUsed = false;
            if (existingConfig != null)
            {
                newConfig.Index = existingConfig.Index + 1;
            }
            else
            {
                newConfig.Index = 1;
            }
            await _uiConfigRepositories.Insert(newConfig);
            return new ResultModel<MessageResultModel>
            {
                StatusCodes = 200,
                Response = new MessageResultModel
                {
                    Message = "Tạo cấu hình thành công!"
                }
            };
        }

        public async Task<ResultModel<MessageResultModel>> UpdateConfig(Guid id, ConfigDto request, string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUserAccount = await _userRepositories.GetSingle(x => x.Id == userId);
            if (getUserAccount == null)
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var GetConfig = await _uiConfigRepositories.GetSingle(x => x.Id == id && x.CreatedBy == userId);
            if (GetConfig == null)
            {
                throw new CustomException("Cấu hình không tồn tại trong hệ thống!");
            }
            GetConfig.Name = request.Name;
            GetConfig.Description = TextConvert.ConvertToUnicodeEscape(request.Description);
            GetConfig.ColorTone = request.ColorTone;
            if (!string.IsNullOrEmpty(request.LogoImage))
            {
                GetConfig.LogoUrl = request.LogoImage;
            }
            if (!string.IsNullOrEmpty(request.BackgroundImage))
            {
                GetConfig.BackgroundUrl = request.BackgroundImage;
            }
            await _uiConfigRepositories.Update(GetConfig);
            return new ResultModel<MessageResultModel>
            {
                StatusCodes = 200,
                Response = new MessageResultModel
                {
                    Message = "Cập nhật cấu hình thành công!"
                }
            };
        }

        public async Task<ConfigImage> GetConfigImage(Guid configId, string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUserAccount = await _userRepositories.GetSingle(x => x.Id == userId);
            if (getUserAccount == null)
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var GetConfig = await _uiConfigRepositories.GetSingle(x => x.Id == configId && x.CreatedBy == userId);
            if (GetConfig == null)
            {
                throw new CustomException("Cấu hình không tồn tại trong hệ thống!");
            }
            return new ConfigImage
            {
                BackgroundUrl = GetConfig.BackgroundUrl,
                LogoUrl = GetConfig.LogoUrl,
            };
        }

        public async Task<ResultModel<MessageResultModel>> ChooseConfig(Guid id, string Token)
        {
            var userId = Guid.Parse(Authentication.DecodeToken(Token, "userid"));
            var getUserAccount = await _userRepositories.GetSingle(x => x.Id == userId);
            if (getUserAccount == null)
            {
                throw new CustomException("Người dùng không tồn tại trong hệ thống!");
            }
            var GetConfig = await _uiConfigRepositories.GetSingle(x => x.Id == id && x.CreatedBy == userId);
            if (GetConfig == null)
            {
                throw new CustomException("Cấu hình không tồn tại trong hệ thống!");
            }
            var allConfigs = await _uiConfigRepositories.GetList(x => x.CreatedBy == userId);
            foreach (var config in allConfigs)
            {
                if (config.Id != id)
                {
                    config.IsUsed = false;
                }
                else
                {
                    config.IsUsed = true;
                }
            }
            await _uiConfigRepositories.UpdateRange(allConfigs.ToList());
            return new ResultModel<MessageResultModel>
            {
                StatusCodes = 200,
                Response = new MessageResultModel
                {
                    Message = "Chọn cấu hình thành công!"
                }
            };
        }
    }
}