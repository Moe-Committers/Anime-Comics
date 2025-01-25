using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace anime_comics.Features.SiteSetting.Commands.Update;

public class UpdateSiteSettingHandler : IRequestHandler<UpdateSiteSetting, Models.SiteSetting>
{
    private readonly database _db;
    private readonly IImageService _imgService;
    private readonly IDistributedCache _cache;
    public UpdateSiteSettingHandler(database db, IImageService imgService , IDistributedCache cache)
    {
        _db = db;
        _imgService = imgService;
        _cache = cache;
    }

    public async Task<Models.SiteSetting> Handle(UpdateSiteSetting request, CancellationToken ct)
    {
        var item = await _db.sitesettings.FirstOrDefaultAsync(s => s.Id == request.Id ,ct);
        if (item == null) throw new NotFoundExceptions("site setting not founded!");
        item.Title = request.Title ?? item.Title;
        if (request.Logo != null)
        {
            if (!string.IsNullOrEmpty(item.Logo))
            {
                _imgService.DeleteImage(item.Logo);
            }
            item.Logo = await _imgService.UploadImage(request.Logo, "site-setting");
        }
        await _db.SaveChangesAsync(ct);

        // After update, invalidate the cache
        var cacheKey = $"site-setting:{item.Id}";
        await _cache.RemoveAsync(cacheKey);

        // Cache the updated item
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(item), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache expires in 30 minutes
        });

        return item.Adapt<Models.SiteSetting>();
    }
}