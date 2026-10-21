using ticketManage.DTOs.Shared;

namespace ticketManage.DTOs.Ticket
{
    public class TicketPageRequest : PageRequest
    {
        public override string[] AllowedSortFields { get; } = ["CreatedAt", "Title"];

        public override string[] AllowedFilterFields { get; } = ["Status", "Priority"];
    }
}
