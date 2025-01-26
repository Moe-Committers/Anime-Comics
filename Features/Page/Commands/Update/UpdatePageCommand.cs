using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Commands.Update;

public record UpdatePageCommand : IRequest<PageDto>
{
    public long ChapterId { get; init; }
    public long PageId { get; init; }
    public int? PageNumber { get; init; }
    public IFormFile? Image { get; init; }
}
