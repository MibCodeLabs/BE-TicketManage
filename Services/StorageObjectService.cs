using ticketManage.Data;
using ticketManage.Models;

namespace ticketManage.Services
{
    public class StorageObjectService
    {
        private AppDbContext _context;

        public StorageObjectService(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<StorageObject[]> AddNewLocalDatabaseObjectsAsync(
        //    byte[][] files,
        //    CancellationToken cancellationToken = default
        //)
        //{
        //    var storageObjects = files.Select(StorageObject.CreateNewLocalDatabaseObject).ToArray();

        //    _context.StorageObjects.AddRange(storageObjects);

        //    await _context.SaveChangesAsync(cancellationToken);

        //    return storageObjects;
        //}

        public async Task<StorageObject> AddNewLocalDatabaseObjectAsync(
            byte[] file,
            CancellationToken cancellationToken = default
        )
        {
            var storageObject = StorageObject.CreateNewLocalDatabaseObject(file);

            await _context.StorageObjects.AddAsync(storageObject);

            await _context.SaveChangesAsync(cancellationToken);

            return storageObject;
        }
    }
}
