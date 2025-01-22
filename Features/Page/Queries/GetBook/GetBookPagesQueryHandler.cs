using anime_comics.DB;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Queries.GetBook;

public class GetBookPagesQueryHandler : IRequestHandler<GetBookPagesQuery, PageResponse<PageDto>>
{
    private readonly database _db;

    public GetBookPagesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<PageResponse<PageDto>> Handle(GetBookPagesQuery request, CancellationToken ct)
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
        return new PageResponse<PageDto>{
            Data = data,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            TotalPage = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
