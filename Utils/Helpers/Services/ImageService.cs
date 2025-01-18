using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;

namespace anime_comics.Utils.Helpers.Services;

public class ImageService : IImageService{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageService> _logger;
    private readonly string[] _isAllowed = {".jpg" , ".jpeg" , ".png" , ".gif"};
    public ImageService(IConfiguration config , IWebHostEnvironment env , ILogger<ImageService> logger){
        _env = env;
        _config = config;
        _logger = logger;
    }
    public async Task<string> UploadImage(IFormFile image , string subDirectory){
        await ValidateImage(image);

        var uploadDirectory = Path.Combine(_env.WebRootPath , "uploads" , subDirectory);
        Directory.CreateDirectory(uploadDirectory);

        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadDirectory , fileName);

        using(var fileStream = new FileStream(filePath , FileMode.Create)){
            image.CopyToAsync(fileStream);
        }

        return $"/uploads/{subDirectory}/{fileName}";
    }
    public void DeleteImage(string path){
        if(string.IsNullOrEmpty(path)) return;
        try{
            var wtf = Path.Combine(_env.WebRootPath , path.TrimStart('/'));
            if(File.Exists(wtf)){
                File.Delete(wtf);
            }
        }catch(Exception ex){
            throw new BadRequestExceptions(ex.Message);
        }
    }
    public async Task<bool> ValidateImage(IFormFile file , int InMbSize = 5){
        if(file.Length == 0 || file == null){
            throw new BadRequestExceptions("invalid image");
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if(!_isAllowed.Contains(ext)){
            throw new BadRequestExceptions($"invalid format only need: {string.Join(", " , _isAllowed)}");
        }

        if(file.Length > InMbSize * 1024 * 1024){
           throw new BadRequestExceptions("image size is too large"); 
        }

        return true;
    }
}