using ticketManage.Utils;

namespace ticketManage.Models
{
    public class StorageObject : AuditableEntity
    {
        public long Id { get; set; }

        public StorageProvider StorageProvider { get; set; }

        // Only populated when StorageProvider=StorageProvider.s3
        public string? StorageKey { get; set; }

        // Only populated when StorageProvider=StorageProvider.Local
        public byte[]? Data { get; set; }

        public bool IsDeleted { get; set; }
    }
}
