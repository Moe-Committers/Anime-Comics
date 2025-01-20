using MediatR;

namespace anime_comics.Features.Book.Commands.Update;

public record UpdateBookCommand : IRequest<bool>
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }
    public required string ImageUrl { get; init; }
    public ICollection<long> CategoryIds { get; init; } = new List<long>();
}
