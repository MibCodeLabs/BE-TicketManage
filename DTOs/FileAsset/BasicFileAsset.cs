using ticketManage.Utils;

namespace ticketManage.DTOs.FileAsset
{
    public class BasicFileAsset
    {
        public string Name { get; set; } = "";
        public string ContentType { get; set; } = "";
        public long Size { get; set; } = 0;
        public string Url { get; set; } = "";

        public static BasicFileAsset Create(string name, string contentType, long size, string url)
        {
            return new BasicFileAsset
            {
                Name = name,
                ContentType = contentType,
                Size = size,
                Url = url,
            };
        }

        public static BasicFileAsset Create(ticketManage.Models.FileAsset asset)
        {
            return Create(
                asset.FileName,
                asset.ContentType,
                asset.Size,
                FileUtils.MakeFileUrl(asset)
            );
        }

        public static List<BasicFileAsset> ConvertToList(List<ticketManage.Models.FileAsset> assets)
        {
            if (Utility.IsNullOrEmpty(assets))
            {
                return [];
            }
            return [.. assets.Select(a => Create(a))];
        }
    }
}
