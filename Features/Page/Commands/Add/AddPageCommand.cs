using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Commands.Add;

public record AddPagesCommand : IRequest<List<PageDto>>
{
    public long ChapterId { get; init; }
    public List<IFormFile> Images { get; init; } = new();
}
