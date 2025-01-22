using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetBook;

public record GetBookPagesQuery : IRequest<PageResponse<PageDto>> {
    public long BookId {get; init;}
    public int Page {get; init; } = 1;
    public int PageSize {get; init;} = 10;
};