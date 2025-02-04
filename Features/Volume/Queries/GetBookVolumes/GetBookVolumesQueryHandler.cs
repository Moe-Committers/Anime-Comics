using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Volumes;
using anime_comics.Utils.Helpers.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Volume.Queries.GetBookVolumes;

public class GetBookVolumesQueryHandler : IRequestHandler<GetBookVolumesQuery, ApiResponse<List<VolumeDto>>>
{
    private readonly database _db;

    public GetBookVolumesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<VolumeDto>>> Handle(GetBookVolumesQuery request, CancellationToken ct)
    {
        var query = _db.volumes
            .Where(v => v.BookId == request.BookId)
            .OrderBy(v => v.VolumeNo)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(v => v.Title.Contains(request.Search)
            || v.VolumeNo == int.Parse(request.Search));
        }

        query = request.sort?.ToLower() switch
        {
            "volumeNo" => request.IsAscending
                ? query.OrderBy(v => v.VolumeNo)
                : query.OrderByDescending(v => v.VolumeNo),
            "title" => request.IsAscending
                ? query.OrderBy(v => v.Title)
                : query.OrderByDescending(v => v.Title),
            "releaseDate" => request.IsAscending
                ? query.OrderBy(v => v.ReleaseDate)
                : query.OrderByDescending(v => v.ReleaseDate),
            "created" => request.IsAscending
                ? query.OrderBy(b => b.CreatedAt)
                : query.OrderByDescending(b => b.CreatedAt),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };

        return await query.CreatePaginatedResponse<VolumeDto, Volumes>(
            request.Page,
            request.PageSize,
            ct
        );
    }
}