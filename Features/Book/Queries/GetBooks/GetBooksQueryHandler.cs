using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
{
    private readonly database _db;

    public GetBooksQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken ct)
    {
        var query = _db.books
            .Include(b => b.Categories)
            .Include(b => b.Users)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(b =>
                b.Title.Contains(request.Search) ||
                b.Description.Contains(request.Search) ||
                b.Author.Contains(request.Search));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(b => b.Categories.Any(c => c.Id == request.CategoryId));
        }

        var books = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return books.Adapt<List<BookDto>>();
    }
}