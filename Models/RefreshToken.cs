using System.Security.Cryptography;
using System.Text;

namespace ticketManage.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static RefreshToken Create(string tokenString, User user)
        {
            return new RefreshToken
            {
                TokenHash = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(tokenString))
                ),
                UserId = user.Id,
                User = user,
            };
        }
    }
}
