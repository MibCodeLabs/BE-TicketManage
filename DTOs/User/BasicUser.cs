using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.DTOs.User
{
    public class BasicUser
    {
        public string FullName { get; set; } = "";
        public string Id { get; set; } = "";

        public static BasicUser Create(string fullName, string id)
        {
            return new BasicUser { FullName = fullName, Id = id };
        }

        public static BasicUser? Create(ticketManage.Models.User user)
        {
            return Create(user.FullName, user.Id);
        }
    }
}
