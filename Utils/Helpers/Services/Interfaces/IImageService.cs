namespace anime_comics.Utils.Helpers.Services.Interfaces;

public interface IImageService{
    Task<string> UploadImage(IFormFile File , string subDirectory);
    void DeleteImage(string path);
    Task<bool> ValidateImage(IFormFile file , int InMbSize = 5);
}