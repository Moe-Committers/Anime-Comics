using MediatR;

namespace anime_comics.Features.Page.Commands.Delete;

public record DeletePagesCommand : IRequest<bool>
{
    public long ChapterId { get; init; }
    public List<long> PageIds { get; init; } = new();
}