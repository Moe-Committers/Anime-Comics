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

        return await query.CreatePaginatedResponse<VolumeDto, Volumes>(
            request.Page,
            request.PageSize,
            ct
        );
    }
}