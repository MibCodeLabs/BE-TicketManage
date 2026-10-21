using ticketManage.Models;

namespace ticketManage.Utils
{
    public static class FileUtils
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return System.Array.Empty<byte>();

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public static string MakeFileUrl(FileAsset fileAsset)
        {
            //we will add swicth statement later for when storage is no longer local
            return String.Concat("/res/", fileAsset.Id);
        }
    }
}
