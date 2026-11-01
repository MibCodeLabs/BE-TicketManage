using ticketManage.DTOs.User;
using ticketManage.Utils;

namespace ticketManage.DTOs.Ticket
{
    public class UpsertTicketRequest
    {
        public long? Id { get; set; }
        public BasicUser? AssignedTo { get; set; }
        public string Content { get; set; } = "";
        public DateTime? ExpiryDate { get; set; }
        public TicketStatus Status { get; set; }
        public string Title { get; set; } = "";
        public IFormFile[]? Attachments { get; set; } = [];
    }
}
