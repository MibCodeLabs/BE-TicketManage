using ticketManage.Utils;

namespace ticketManage.Models
{
    public class StorageObject : AuditableEntity
    {
        public long Id { get; set; }

        //TODO change this from int to string in DB when decided on security strategy
        public StorageProvider StorageProvider { get; set; }

        // Only populated when StorageProvider=StorageProvider.s3
        public string? StorageKey { get; set; }

        // Only populated when StorageProvider=StorageProvider.Database
        public byte[]? Data { get; set; }

        public bool IsDeleted { get; set; }

        public static StorageObject CreateNewLocalDatabaseObject(byte[]? data)
        {
            return new StorageObject
            {
                StorageProvider = StorageProvider.Database,
                Data = data,
                IsDeleted = false,
                StorageKey = null,
            };
        }
    }
}
