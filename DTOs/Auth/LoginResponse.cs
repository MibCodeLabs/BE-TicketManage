namespace ticketManage.DTOs.Auth
{
    public class LoginResponse
    {
        public string Message { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public LoggedInUserResponse User { get; set; } = null!;
    }
}
