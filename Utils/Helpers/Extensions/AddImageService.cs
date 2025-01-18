using anime_comics.Utils.Helpers.Services;
using anime_comics.Utils.Helpers.Services.Interfaces;

namespace anime_comics.Utils.Helpers.Extensions;

public static class AddImageService {
     public static IServiceCollection UseImageService(this IServiceCollection app) {
        var services = app.AddScoped<IImageService , ImageService>();
        return services;
     }
}