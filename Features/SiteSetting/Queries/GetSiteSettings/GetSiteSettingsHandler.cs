using System.Text.Json;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace anime_comics.Features.SiteSetting.Queries.GetSiteSettings;

public class GetSiteSettingsHandler : IRequestHandler<GetSiteSettings, List<Models.SiteSetting>>
{
    private readonly database _db;
    private readonly IDistributedCache _cache;

    public GetSiteSettingsHandler(database db, IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<Models.SiteSetting>> Handle(GetSiteSettings request, CancellationToken ct)
    {
        var cacheKey = "all-site-settings";  // A general cache key for all site settings

        // Check if the data is in cache
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            // Return the cached result
            return JsonSerializer.Deserialize<List<Models.SiteSetting>>(cachedData);
        }

        // If not in cache, fetch from the database
        var data = await _db.sitesettings.ToListAsync(ct);

        // Cache the result before returning it
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)  // Cache expiration time
        });

        return data;
    }
}