using ticketManage.DTOs.FileAsset;
using ticketManage.DTOs.User;
using ticketManage.Utils;

namespace ticketManage.DTOs.Ticket
{
    public class TicketResponse
    {
        public long? Id { get; set; }
        public TicketStatus Status { get; set; }
        public BasicUser IssuedByUser { get; set; } = null!;
        public BasicUser? AssignedTo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Content { get; set; } = "";
        public string Title { get; set; } = "";
        public List<BasicFileAsset>? Attachments { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}
