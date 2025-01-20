using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDetailDto>
{
    private readonly database _db;

    public GetCategoryQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<CategoryDetailDto> Handle(GetCategoryQuery request, CancellationToken ct)
    {
        var category = await _db.categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Users)
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category == null)
            throw new NotFoundExceptions("Category not found");

        return new CategoryDetailDto
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.Books.Select(b => new BookDto
            {
                Id = b.id,
                Title = b.Title,
                Author = b.Author,
                ImageUrl = b.ImageUrl,
                UserName = b.Users.Name
            }).ToList()
        };
    }
}