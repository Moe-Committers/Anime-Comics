using anime_comics.DB;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
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
        var totalCount = await _db.pages.CountAsync();
        var pages = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).OrderBy(p => p.PageNumber)
            .Select(p => new PageDto
            {
                Id = p.Id,
                BookId = p.BookId,
                PageNumber = p.PageNumber,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync(ct);
        var data = pages.Adapt<List<PageDto>>();
        return new ApiResponse<List<PageDto>>
        {
            Data = data,
            Paginate = new PaginateResponse
            {
                TotalCount = totalCount,
                PageNumber = request.Page,
                PageSize = request.PageSize,
                TotalPage = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            }
        };
    }
}
