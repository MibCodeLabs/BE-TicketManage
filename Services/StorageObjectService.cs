using ticketManage.Data;
using ticketManage.Exceptions;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class StorageObjectService
    {
        private AppDbContext _context;

        public StorageObjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StorageObject> AddNewLocalDatabaseObjectAsync(
            byte[] file,
            CancellationToken cancellationToken = default
        )
        {
            //add exhaustive verification for magic byte in future to avoid malicious uploads
            var storageObject = StorageObject.CreateNewLocalDatabaseObject(file);

            await _context.StorageObjects.AddAsync(storageObject);

            await _context.SaveChangesAsync(cancellationToken);

            return storageObject;
        }

        public byte[] GetStorageObjectData(StorageObject storageObject)
        {
            switch (storageObject.StorageProvider)
            {
                case StorageProvider.Database:
                {
                    return this.GetLocalFileData(storageObject);
                }
                case StorageProvider.S3:
                {
                    throw new AppException("Not implemented");
                }
                default:
                {
                    throw new AppException("Wrong Type");
                }
            }
        }

        public byte[] GetLocalFileData(StorageObject storageObject)
        {
            if (Utility.IsNullOrEmpty(storageObject.Data))
            {
                throw new NotFoundException("Data not found,contact support");
            }
            return storageObject.Data;
        }
    }
}
