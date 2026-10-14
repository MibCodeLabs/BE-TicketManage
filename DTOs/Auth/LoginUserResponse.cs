using ticketManage.Utils;

namespace ticketManage.DTOs.Auth
{
    public class LoginUserResponse
    {
        public string FullName { get; set; } = "";
        public string AccountType { get; set; } = "";
        public string ProfilePicture { get; set; } = "";
    }
}
