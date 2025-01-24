using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Helpers.ResponseHelper;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Queries.GetBook;

public class GetBookPagesQueryHandler : IRequestHandler<GetBookPagesQuery, ApiResponse<List<PageDto>>>
{
    private readonly database _db;

    public GetBookPagesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<PageDto>>> Handle(GetBookPagesQuery request, CancellationToken ct)
    {
        var query = _db.pages.Where(p => p.BookId == request.BookId).AsQueryable();
        return await query.CreatePaginatedResponse<PageDto , Pages>(request.Page , request.PageSize , ct);
    }
}
