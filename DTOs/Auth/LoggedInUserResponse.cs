using ticketManage.Utils;

namespace ticketManage.DTOs.Auth
{
    public class LoggedInUserResponse
    {
        public string FullName { get; set; } = "";
        public string AccountType { get; set; } = "";
        public string ProfilePicture { get; set; } = "";
    }
}
