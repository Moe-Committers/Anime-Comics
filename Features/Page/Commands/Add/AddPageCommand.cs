using MediatR;

namespace anime_comics.Features.Page.Commands.Add;

public record AddPageCommand : IRequest<List<long>>
{
    public long BookId { get; init; }
    public List<IFormFile> ImageUrl { get; init; } = new();
}