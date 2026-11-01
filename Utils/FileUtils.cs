using ticketManage.Models;
using static System.Net.Mime.MediaTypeNames;

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

        public static string GetMimeType(AllowedMimeType type) =>
            type switch
            {
                AllowedMimeType.Pdf => "application/pdf",
                AllowedMimeType.Png => "image/png",
                AllowedMimeType.Svg => "image/svg+xml",
                AllowedMimeType.Jpeg => "image/jpeg",
                AllowedMimeType.Webp => "image/webp",
                AllowedMimeType.PlainText => "text/plain",
                AllowedMimeType.Csv => "text/csv",
                AllowedMimeType.Zip => "application/zip",
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };

        public static string CheckMimeType(AllowedMimeType type) =>
            type switch
            {
                AllowedMimeType.Pdf => "application/pdf",
                AllowedMimeType.Png => "image/png",
                AllowedMimeType.Svg => "image/svg+xml",
                AllowedMimeType.Jpeg => "image/jpeg",
                AllowedMimeType.Webp => "image/webp",
                AllowedMimeType.PlainText => "text/plain",
                AllowedMimeType.Csv => "text/csv",
                AllowedMimeType.Zip => "application/zip",
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };

        public static void ValidateMimeType(string contentType)
        {
            var allowedMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "application/pdf",
                "image/png",
                "image/svg+xml",
                "image/jpeg",
                "image/webp",
                "text/plain",
                "text/csv",
                "application/zip",
            };

            if (!allowedMimeTypes.Contains(contentType))
            {
                throw new BadHttpRequestException($"Unsupported MIME type: {contentType}");
            }
        }
    }
}
