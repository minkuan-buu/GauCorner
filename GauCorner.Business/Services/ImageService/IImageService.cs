using Microsoft.AspNetCore.Http;

public interface IImageService
{
    Task DeleteImages(List<string> imageUrls, string folderPath, string subFolder, string token);
    Task<List<string>> SaveUploadedFiles(IFormFile[] files, string folderPath, string urlPrefix, string subFolder, string token);
}