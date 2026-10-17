using ticketManage.Data;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Services
{
    public class FileAssetService
    {
        private readonly AppDbContext _context;

        public FileAssetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileAsset[]> AddLocalDatabaseFileAssetsAsync(
            IFormFile[] files,
            FileAssetPurpose purpose,
            CancellationToken cancellationToken = default
        )
        {
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
    }
}
