namespace ticketManage.DTOs.Auth
{
    public class LoginApiResponse
    {
        public string Message { get; set; } = "";
        public LoginUserResponse User { get; set; } = null!;
    }
}
