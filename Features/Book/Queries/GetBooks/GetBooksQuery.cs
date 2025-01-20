using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetBooks;

public record GetBooksQuery : IRequest<PageResponse<BookDto>>
{
    public string? Search { get; init; }
    public long? CategoryId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}