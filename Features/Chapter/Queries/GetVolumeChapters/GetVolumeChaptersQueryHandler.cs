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

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(c => c.Title.Contains(request.Search) || c.PageCount == int.Parse(request.Search));
        }

        query = request.sort?.ToLower() switch
        {
            "chapNo" => request.IsAscending
                ? query.OrderBy(c => c.ChapNo)
                : query.OrderByDescending(v => v.ChapNo),
            "title" => request.IsAscending
                ? query.OrderBy(v => v.Title)
                : query.OrderByDescending(v => v.Title),
            "releaseDate" => request.IsAscending
                ? query.OrderBy(v => v.ReleaseDate)
                : query.OrderByDescending(v => v.ReleaseDate),
            "pageCount" => request.IsAscending
                ? query.OrderBy(v => v.PageCount)
                : query.OrderByDescending(v => v.PageCount),
            "created" => request.IsAscending
                ? query.OrderBy(b => b.CreatedAt)
                : query.OrderByDescending(b => b.CreatedAt),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };

        return await query.CreatePaginatedResponse<ChapterDto, Chapters>(
            request.Page,
            request.PageSize,
            ct
        );
    }
}