namespace GauCorner.Data.DTO.ResponseModel.UserResModel
{
    public class UserLoginResModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Attachment { get; set; }
        public string Status { get; set; } = null!;
        public UserAuthResModel Auth { get; set; } = null!;
    }

    public class UserAuthResModel
    {
        public Guid DeviceId { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class CreateHashPasswordModel
    {
        public byte[] HashedPassword { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
    }
}