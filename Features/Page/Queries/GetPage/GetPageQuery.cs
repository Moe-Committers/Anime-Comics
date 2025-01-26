using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetPage;

public record GetPageQuery : IRequest<PageDto>
{
    public long PageId { get; init; }
}
