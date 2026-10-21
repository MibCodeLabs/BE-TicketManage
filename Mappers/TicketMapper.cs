using Riok.Mapperly.Abstractions;
using ticketManage.DTOs.Ticket;
using ticketManage.Models;

namespace ticketManage.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(UserMapper))]
    [UseStaticMapper(typeof(FileAssetMapper))]
    public static partial class TicketMapper
    {
        public static partial TicketResponse toTicketResponse(Ticket ticket);
    }
}
