using anime_comics.DB;
using anime_comics.Utils.DTOs.Category;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly database _db;

    public GetCategoriesQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _db.categories
            .Include(c => c.Books)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                BookCount = c.Books.Count
            })
            .ToListAsync(ct);

        return categories;
    }
}