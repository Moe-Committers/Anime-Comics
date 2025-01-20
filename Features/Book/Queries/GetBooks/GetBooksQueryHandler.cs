using anime_comics.DB;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PageResponse<BookDto>>
{
    private readonly database _db;

    public GetBooksQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<PageResponse<BookDto>> Handle(GetBooksQuery request, CancellationToken ct)
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

        var totalCount = await _db.books.CountAsync();

        var books = await query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(b => new BookDto
        {
            Id = b.id,
            Title = b.Title,
            Description = b.Description,
            Author = b.Author,
            ImageUrl = b.ImageUrl,
            UserName = b.Users.Name,
            Fav = b.Fav,
            Categories = b.Categories.Select(c => new Cate
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
        })
        .ToListAsync(ct);

        var data = books.Adapt<List<BookDto>>();

        return new PageResponse<BookDto> {
            Data = data,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            TotalPage = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}