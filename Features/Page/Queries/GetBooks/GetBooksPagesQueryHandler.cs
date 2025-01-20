using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Queries.GetBooks;

public class GetPageQueryHandler : IRequestHandler<GetPageQuery, PageDto>
{
    private readonly database _db;

    public GetPageQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<PageDto> Handle(GetPageQuery request, CancellationToken ct)
    {
        var page = await _db.pages
            .FirstOrDefaultAsync(p => p.id == request.Id, ct);

        if (page == null)
            throw new NotFoundExceptions("Page not found");

        return new PageDto
        {
            Id = page.id,
            BookId = page.BookId,
            PageNumber = page.PageNumber,
            ImageUrl = page.ImageUrl
        };
    }
}