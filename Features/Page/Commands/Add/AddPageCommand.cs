using MediatR;

namespace anime_comics.Features.Page.Commands.Add;

public record AddPageCommand : IRequest<long>
{
    public long BookId { get; init; }
    public int PageNumber { get; init; }
    public string ImageUrl { get; init; }
}