using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using ticketManage.Data;
using ticketManage.Exceptions;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class FileAssetService
    {
        private readonly StorageObjectService _storageObjectService;
        private readonly AppDbContext _context;

        public FileAssetService(AppDbContext context, StorageObjectService storageObjectService)
        {
            _context = context;
            _storageObjectService = storageObjectService;
        }

        public async Task<FileAsset[]> AddLocalDatabaseFileAssetsAsync(
            IFormFile[] files,
            FileAssetPurpose purpose,
            CancellationToken cancellationToken = default
        )
        {
            //add mimetype verification later
            if (files == null || files.Length == 0)
            {
                return [];
            }
            var fileAssets = await Task.WhenAll(
                files.Select(async file =>
                {
                    var storageObject = StorageObject.CreateNewLocalDatabaseObject(
                        await FileUtils.ToByteArrayAsync(file)
                    );

                    return FileAsset.CreateNewLocalDatabaseFileAsset(
                        purpose,
                        storageObject,
                        file.Length,
                        file.ContentType,
                        file.FileName
                    );
                })
            );

            await _context.FileAssets.AddRangeAsync(fileAssets);

            await _context.SaveChangesAsync(cancellationToken);

            return fileAssets;
        }

        internal async Task<(FileAsset FileAsset, byte[] Data)> GetFile(long id)
        {
            var file =
                await _context
                    .FileAssets.Include(f => f.StorageObject)
                    .FirstOrDefaultAsync(f => f.Id == id)
                ?? throw new NotFoundException("Ticket Not found");

            var storageObject = file.StorageObject;
            if (Utility.IsNullOrEmpty(storageObject))
            {
                throw new NotFoundException("Data not found");
            }

            return (file, _storageObjectService.GetStorageObjectData(storageObject));
        }
    }
}
