using System.Text.Json;
using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace anime_comics.Features.SiteSetting.Queries.GetSiteSetting;

public class GetSiteSettingHandler : IRequestHandler<GetSiteSetting, anime_comics.Models.SiteSetting>
{
    private readonly database _db;
    private readonly IDistributedCache _cache;

    public GetSiteSettingHandler(database db, IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<anime_comics.Models.SiteSetting> Handle(GetSiteSetting request, CancellationToken ct)
    {
        var cacheKey = $"site-setting:{request.Id}";  // Cache key based on the site setting ID

        // Check if the data is in cache
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            // Return the cached result
            return JsonSerializer.Deserialize<anime_comics.Models.SiteSetting>(cachedData);
        }

        // If not in cache, fetch from the database
        var data = await _db.sitesettings.FirstOrDefaultAsync(s => s.Id == request.Id, ct);
        if (data == null) throw new NotFoundExceptions("Item not found!");

        // Cache the result before returning it
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)  // Cache expiration time
        });

        return data.Adapt<anime_comics.Models.SiteSetting>();
    }
}