using ticketManage.Utils;

namespace ticketManage.Models
{
    public class FileAsset : AuditableEntity
    {
        public long Id { get; set; }

        public string FileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long Size { get; set; }

        public long StorageObjectId { get; set; }

        public StorageObject StorageObject { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public FileAssetPurpose Purpose { get; set; }
        public string Note { get; set; }
    }
}
