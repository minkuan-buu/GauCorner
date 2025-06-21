using GauCorner.Data.DTO.RequestModel;
using GauCorner.Data.DTO.ResponseModel;
using GauCorner.Data.DTO.ResponseModel.ResultModel;

namespace GauCorner.Business.Services.DonateServices
{
    public interface IDonateServices
    {
        Task<ResultModel<MessageResultModel>> CreateDonate(DonateReqModel donateModel, string userPath);
        Task<ResultModel<DonatePageResModel>> GetDonatePage(string userPath);
        Task<ResultModel<TransactionStatusResModel>> HandleCheckTransaction(string apptransid);
        Task<ResultModel<ListDataResultModel<DonatePageConfigLabel>>> GetConfigLabel(string Token);
        Task<ResultModel<DataResultModel<DonatePageConfigResModel>>> GetConfigById(Guid configId, string Token);
        Task<ResultModel<MessageResultModel>> CreateConfig(ConfigDto request, string Token);
        Task<ResultModel<MessageResultModel>> UpdateConfig(Guid id, ConfigDto request, string Token);
        Task<ConfigImage> GetConfigImage(Guid configId, string Token);
        // Task<bool> UpdateDonate(DonateModel donateModel);
        // Task<bool> DeleteDonate(int id);
        // Task<DonateModel> GetDonateById(int id);
        // Task<List<DonateModel>> GetAllDonates();
        // Task<List<DonateModel>> GetAllDonatesByUserId(string userId);
    }
}