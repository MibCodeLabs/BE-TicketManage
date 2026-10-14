namespace ticketManage.Models
{
    public class Chat : AuditableEntity
    {
        public long Id { get; set; }
        public string Content { get; set; } = "";
        public FileAsset? Attachment { get; set; }
        public string FromUserId { get; set; }
        public User FromUser { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
