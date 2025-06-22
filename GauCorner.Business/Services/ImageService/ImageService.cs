using GauCorner.Business.Utilities.Authentication;
using Microsoft.AspNetCore.Http;

public class ImageService : IImageService
{
    public Task DeleteImages(List<string> imageUrls, string folderPath, string subFolder, string token)
    {
        var userId = Authentication.DecodeToken(token, "userid");
        foreach (var url in imageUrls)
        {
            var fileName = Path.GetFileName(new Uri(url).LocalPath);
            var filePath = Path.Combine(folderPath, userId, subFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        return Task.CompletedTask;
    }

    public async Task<List<string>> SaveUploadedFiles(IFormFile[] files, string folderPath, string urlPrefix, string subFolder, string token)
    {
        var userId = Authentication.DecodeToken(token, "userid");

        // Cấu hình folder động: /uploads/shop/products/{userId}
        var userFolderPath = Path.Combine(folderPath, userId, subFolder);
        var userUrlPrefix = $"{urlPrefix}/{userId}/{subFolder}";

        var savedUrls = new List<string>();

        foreach (var file in files)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(userFolderPath, fileName);

            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            savedUrls.Add($"{userUrlPrefix}/{fileName}");
        }

        return savedUrls;
    }
}