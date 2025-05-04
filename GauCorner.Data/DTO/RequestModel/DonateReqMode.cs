namespace GauCorner.Data.DTO.RequestModel
{
    public class DonateReqModel
    {
        public string Username { get; set; } = null!;
        public string Message { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}