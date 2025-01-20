using anime_comics.Utils.DTOs.Category;
using MediatR;

namespace anime_comics.Features.Category.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;