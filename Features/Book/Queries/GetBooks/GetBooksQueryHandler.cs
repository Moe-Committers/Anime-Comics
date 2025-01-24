using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Extensions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, ApiResponse<List<BookDto>>>
{
    private readonly database _db;

    public GetBooksQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<BookDto>>> Handle(GetBooksQuery request, CancellationToken ct)
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
                query = query.Where(b => b.Published_at >= DateTime.UtcNow.Date);
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

        return await query.CreatePaginatedResponse<BookDto , Books>(request.Page , request.PageSize , ct);
    }
}