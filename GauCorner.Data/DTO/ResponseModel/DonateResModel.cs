using Microsoft.Identity.Client.Advanced;

namespace GauCorner.Data.DTO.ResponseModel
{
    public class DonatePageResModel
    {
        public string UserPath { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string BackgroundUrl { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string ColorTone { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class DonatePageConfigLabel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsUsed { get; set; }
    }

    public class DonatePageConfigResModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string BackgroundUrl { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string ColorTone { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class TransactionStatusResModel
    {
        public int ReturnCode { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public decimal Amount { get; set; }
        public bool isProcessing { get; set; }
    }

    public class ConfigImage
    {
        public string BackgroundUrl { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
    }
}