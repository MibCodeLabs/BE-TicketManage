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
        public string Note { get; set; } = "";

        public static FileAsset CreateNewLocalDatabaseFileAsset(
            FileAssetPurpose purpose,
            StorageObject storageObject,
            long size,
            string contentType,
            string fileName
        )
        {
            return new FileAsset
            {
                Note = "Saved File Locally in DB for purpose of " + purpose.ToString(),
                IsDeleted = false,
                Purpose = purpose,
                StorageObject = storageObject,
                StorageObjectId = storageObject.Id,
                Size = size,
                ContentType = contentType,
                FileName = fileName,
            };
        }
    }
}
