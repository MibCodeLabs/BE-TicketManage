using Riok.Mapperly.Abstractions;
using ticketManage.DTOs.User;
using ticketManage.Models;

namespace ticketManage.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        public static partial BasicUser toBasicUser(User user);
    }
}
