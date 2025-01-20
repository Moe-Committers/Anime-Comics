using MediatR;

namespace anime_comics.Features.Book.Commands.Create;

public record CreateBookCommand : IRequest<long>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Author { get; init; }
    public string ImageUrl { get; init; }
    public ICollection<long> CategoryIds { get; init; } = new List<long>();
}
