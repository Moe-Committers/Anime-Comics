using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Chapters;
using anime_comics.Utils.Helpers.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Chapter.Queries.GetVolumeChapters;

public class GetVolumeChaptersQueryHandler : IRequestHandler<GetVolumeChaptersQuery, ApiResponse<List<ChapterDto>>>
{
    private readonly database _db;

    public GetVolumeChaptersQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<ChapterDto>>> Handle(GetVolumeChaptersQuery request, CancellationToken ct)
    {
        var query = _db.Chapters.AsQueryable();

        query = query.Where(c => c.VolumeId == request.VolumeId)
                    .OrderBy(c => c.ChapNo);

        return await query.CreatePaginatedResponse<ChapterDto, Chapters>(
            request.Page,
            request.PageSize,
            ct
        );
    }
}