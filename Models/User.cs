using Microsoft.AspNetCore.Identity;
using ticketManage.Utils;

namespace ticketManage.Models
{
    public class User : IdentityUser
    {
        public string ProfilePicture { get; set; } = "/images/default-avatar.svg";
        public string FullName { get; set; } = null!;
        public AccountType AccountType { get; set; } = AccountType.Employee;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "System";
        public string? ModifiedBy { get; set; } = "System";
        public bool IsDeleted { get; set; } = false;
    }
}
