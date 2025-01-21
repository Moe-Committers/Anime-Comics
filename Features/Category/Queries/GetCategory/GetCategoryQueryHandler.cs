using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
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
        var categoryQuery = _db.categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Users)
            .Where(c => c.Id == request.Id);

        if (request.Toggle)
        {
            categoryQuery = categoryQuery.Where(c => c.status == Status.Active);
        }

        var category = await categoryQuery.Select(c => new CategoryDetailDto
        {
            Id = c.Id,
            Name = c.Name,
            Books = c.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ImageUrl = b.ImageUrl,
                UserName = b.Users.Name
            }).ToList()
        }).FirstOrDefaultAsync();

        if (category == null)
            throw new NotFoundExceptions("Category not found");

        return category.Adapt<CategoryDetailDto>();
    }
}