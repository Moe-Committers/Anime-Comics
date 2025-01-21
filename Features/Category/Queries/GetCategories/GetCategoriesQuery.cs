using System.Text.Json.Serialization;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Enum;
using MediatR;

namespace anime_comics.Features.Category.Queries.GetCategories;

public record GetCategoriesQuery : IRequest<PageResponse<CategoryDto>>{
    [JsonIgnore]
    public bool Toggle {get; set;}
    public string? Search {get; set;}
    public long? BookId {get; set;}
    public DateTime? FromDate {get; set;}
    public DateTime? ToDate {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
    public int Page {get; init; } = 1;
    public int PageSize {get; init;} = 10;
};