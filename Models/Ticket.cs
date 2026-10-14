using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Models
{
    public class Ticket : AuditableEntity
    {
        public long Id { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Unassigned;
        public required string IssuedByUserId { get; set; }
        public User IssuedByUser { get; set; } = null!;
        public DateTime? Expiry { get; set; }
        public string Content { get; set; } = "";

        public string? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
        public ICollection<FileAsset>? Attachments { get; set; }
        public bool IsDeleted { get; set; }
    }
}
