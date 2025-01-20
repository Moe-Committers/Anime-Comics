using MediatR;

namespace anime_comics.Features.Book.Commands.Update;

public record UpdateBookCommand : IRequest<bool>
{
    public long Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Author { get; init; }
    public string ImageUrl { get; init; }
    public ICollection<long> CategoryIds { get; init; } = new List<long>();
}
