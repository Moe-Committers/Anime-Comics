using System.Text.Json.Serialization;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetBooks;

public record GetBooksQuery : IRequest<PageResponse<BookDto>>
{
    [JsonIgnore]
    public bool pulished {get; set;}
    public bool isLatest {get; set;} = false;
    public string? Search { get; init; }
    public long? CategoryId { get; init; }
    public DateTime? FromDate {get; set;}
    public DateTime? ToDate {get; set;}
    public DateTime? Published_at {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}