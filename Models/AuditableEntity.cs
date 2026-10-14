namespace ticketManage.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = "System";
        public string? ModifiedBy { get; set; } = "System";
    }
}
