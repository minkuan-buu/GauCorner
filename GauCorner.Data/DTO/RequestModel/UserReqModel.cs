namespace GauCorne.Data.DTO.RequestModel
{
    public class UserRegisterModel
    {
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserLoginModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}