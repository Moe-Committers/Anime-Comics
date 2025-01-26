using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetChapterPages;

public record GetChapterPagesQuery : IRequest<ApiResponse<List<PageDto>>>
{
    public long ChapterId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}