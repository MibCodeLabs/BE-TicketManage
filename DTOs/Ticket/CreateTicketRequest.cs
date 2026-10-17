using ticketManage.Utils;

namespace ticketManage.DTOs.Ticket
{
    public class CreateTicketRequest
    {
        public string? AssignedTo { get; set; }
        public string Description { get; set; } = "";
        public DateTime? ExpiryDate { get; set; }
        public TicketStatus Status { get; set; }
        public string Title { get; set; } = "";
        public IFormFile[]? Attachments { get; set; } = [];
    }
}
