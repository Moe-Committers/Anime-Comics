using MediatR;

namespace anime_comics.Features.Page.Commands.Update;

public record UpdatePageCommand : IRequest<bool>
{
    public long Id { get; init; }
    public long BookId { get; init; }
    public int PageNumber { get; init; }
    public string ImageUrl { get; init; }
}