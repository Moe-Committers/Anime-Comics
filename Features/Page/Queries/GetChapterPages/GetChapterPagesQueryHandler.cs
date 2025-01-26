using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Extensions;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetChapterPages;

public class GetChapterPagesQueryHandler : IRequestHandler<GetChapterPagesQuery, ApiResponse<List<PageDto>>>
{
    private readonly database _db;

    public GetChapterPagesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<PageDto>>> Handle(GetChapterPagesQuery request, CancellationToken ct)
    {
        var query = _db.pages
            .Where(p => p.ChapterId == request.ChapterId)
            .OrderBy(p => p.PageNumber)
            .AsQueryable();

        return await query.CreatePaginatedResponse<PageDto, Pages>(
            request.Page,
            request.PageSize,
            ct
        );
    }
}