using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Helpers.ResponseHelper;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ApiResponse<List<CategoryDto>>>
{
    private readonly database _db;

    public GetCategoriesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ApiResponse<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var query = _db.categories
            .Include(c => c.Books)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(c => c.Name.ToLower().Contains(request.Search));
        }

        if (request.BookId.HasValue)
        {
            query = query.Where(c => c.Books.Any(b => b.Id == request.BookId));
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(c => c.CreatedAt >= request.FromDate);
        }

        if (request.Toggle)
        {
            query = query.Where(c => c.status == Status.Active);
            query = query.OrderBy(c => c.Order);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(c => c.CreatedAt <= request.ToDate);
        }

        query = request.sort?.ToLower() switch
        {
            "name" => request.IsAscending
                ? query.OrderBy(c => c.Name)
                : query.OrderByDescending(c => c.Name),
            "status" => request.IsAscending
                ? query.OrderBy(c => c.status)
                : query.OrderByDescending(c => c.status),
            "created" => request.IsAscending
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        return await query.CreatePaginatedResponse<CategoryDto , Categories>(request.Page , request.PageSize , ct);
    }
}