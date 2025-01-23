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

        if (request.FromDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt >= request.FromDate);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(b => b.CreatedAt <= request.ToDate);
        }

        if (request.pulished)
        {
            query = query.Where(b => b.Published_at != null);
            if (request.isLatest)
            {
                query = query.Where(b => b.Published_at >= DateTime.UtcNow);
            }
        }


        query = request.sort?.ToLower() switch
        {
            "title" => request.IsAscending
                ? query.OrderBy(b => b.Title)
                : query.OrderByDescending(b => b.Title),
            "author" => request.IsAscending
                ? query.OrderBy(b => b.Author)
                : query.OrderByDescending(b => b.Author),
            "fav" => request.IsAscending
                ? query.OrderBy(b => b.Fav)
                : query.OrderByDescending(b => b.Fav),
            "created" => request.IsAscending
                ? query.OrderBy(b => b.CreatedAt)
                : query.OrderByDescending(b => b.CreatedAt),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };

        var totalCount = await _db.books.CountAsync();

        var books = await query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            ImageUrl = b.ImageUrl,
            Published_at = b.Published_at,
            UserName = b.Users.Name,
            Fav = b.Fav
        })
        .ToListAsync(ct);

        var data = books.Adapt<List<BookDto>>();

        return new PageResponse<BookDto>
        {
            Data = data,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            TotalPage = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}