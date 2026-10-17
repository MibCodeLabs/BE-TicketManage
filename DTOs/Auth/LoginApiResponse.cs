namespace ticketManage.DTOs.Auth
{
    public class LoginApiResponse
    {
        public string Message { get; set; } = "";
        public LoggedInUserResponse User { get; set; } = null!;
    }
}
