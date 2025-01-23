using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetPopularBook;

public class GetPopularBookQueryHandler : IRequestHandler<GetPopularBookQuery, List<BookDetailDto>>
{
    private readonly database _db;
    public GetPopularBookQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<List<BookDetailDto>> Handle(GetPopularBookQuery request, CancellationToken ct)
    {
        var data = await _db.books
        .Where(b => b.Published_at != null)
        .OrderByDescending(b => b.Fav)
        .Include(b => b.Categories)
        .Include(b => b.Users)
        .Select(b => new BookDetailDto
        {
            Id = b.Id,
            Title = b.Title,
            Description = b.Description,
            Author = b.Author,
            Published_at = b.Published_at,
            ImageUrl = b.ImageUrl,
            UserName = b.Users.Name,
            Fav = b.Fav,
            Categories = b.Categories.Select(c => new Cate
            {
                Id = c.Id,
                Name = c.Name,
                Order = c.Order
            }).ToList()
        })
        .Take(request.Limited)
        .ToListAsync(ct);

        return data.Adapt<List<BookDetailDto>>();
    }
}