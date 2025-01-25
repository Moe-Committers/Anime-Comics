using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace anime_comics.Features.SiteSetting.Commands.Delete;

public class DeleteSiteSettingHandler : IRequestHandler<DeleteSiteSetting, bool>
{
    private readonly database _db;
    private readonly IImageService _imgService;
    private readonly IDistributedCache _cache;
    public DeleteSiteSettingHandler(database db, IImageService imgService, IDistributedCache cache)
    {
        _db = db;
        _imgService = imgService;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteSiteSetting request, CancellationToken ct)
    {
        var data = await _db.sitesettings.FirstOrDefaultAsync(s => s.Id == request.Id, ct);
        if (data == null) return false;
        if (!string.IsNullOrEmpty(data.Logo)) _imgService.DeleteImage(data.Logo);
        _db.sitesettings.Remove(data);
        await _db.SaveChangesAsync(ct);

        var cacheKey = $"site-setting:{data.Id}";
        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}