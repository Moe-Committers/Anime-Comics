using System.Text.Json.Serialization;
using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace anime_comics.Features.SiteSetting.Commands.Add;

public class AddSiteSettingHandler : IRequestHandler<AddSiteSetting , Models.SiteSetting> {
    private readonly database _db;
    private readonly IImageService _imgService;
    private readonly IDistributedCache _cache;
    public AddSiteSettingHandler(database db , IImageService imgService , IDistributedCache cache){
        _db = db;
        _imgService = imgService;
        _cache = cache;
    }

    public async Task<Models.SiteSetting> Handle(AddSiteSetting request , CancellationToken ct){
        var item = new Models.SiteSetting {
            Title = request.Title,
            Logo = await _imgService.UploadImage(request.Logo , "site-setting")
        };
        _db.sitesettings.Add(item);
        await _db.SaveChangesAsync(ct);
        var cacheKey = $"site-setting:{item.Id}";
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(item), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

        var data = item.Adapt<Models.SiteSetting>();
        return data;
    }
}