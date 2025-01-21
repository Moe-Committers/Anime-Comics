using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Queries.GetBook;

public class GetBookPagesQueryHandler : IRequestHandler<GetBookPagesQuery, List<PageDto>>
{
    private readonly database _db;

    public GetBookPagesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<List<PageDto>> Handle(GetBookPagesQuery request, CancellationToken ct)
    {
        var pages = await _db.pages
            .Where(p => p.BookId == request.BookId)
            .OrderBy(p => p.PageNumber)
            .Select(p => new PageDto
            {
                Id = p.Id,
                BookId = p.BookId,
                PageNumber = p.PageNumber,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync(ct);

        return pages;
    }
}
