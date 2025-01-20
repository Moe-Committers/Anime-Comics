using anime_comics.Utils.DTOs.Category;
using MediatR;

namespace anime_comics.Features.Category.Queries.GetCategory;

public record GetCategoryQuery(long Id) : IRequest<CategoryDetailDto>;