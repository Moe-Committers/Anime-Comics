using System.Text.Json.Serialization;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Enum;
using MediatR;

namespace anime_comics.Features.Category.Queries.GetCategory;

public record GetCategoryQuery : IRequest<CategoryDetailDto> {
    public long Id {get; set;}
    [JsonIgnore]
    public bool Toggle {get; set;}
};