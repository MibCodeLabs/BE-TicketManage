using Riok.Mapperly.Abstractions;
using ticketManage.DTOs.FileAsset;
using ticketManage.Models;
using ticketManage.Utils;

namespace ticketManage.Mappers
{
    [Mapper]
    public static partial class FileAssetMapper
    {
        [MapProperty(nameof(FileAsset.FileName), nameof(BasicFileAsset.Name))]
        public static partial BasicFileAsset ToBasicFileAsset(FileAsset fileAsset);

        [MapPropertyFromSource(nameof(BasicFileAsset.Url))]
        private static string MapUrl(FileAsset fileAsset)
        {
            return FileUtils.MakeFileUrl(fileAsset);
        }
    }
}
