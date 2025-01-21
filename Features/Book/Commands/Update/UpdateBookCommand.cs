using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.Book.Commands.Update;

public record UpdateBookCommand : IRequest<bool>
{   
    [JsonIgnore]
    public long Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; set; }
    public string? Author { get; init; }
    public string? ImageUrl { get; init; }
    public ICollection<long>? CategoryIds { get; init; } = new List<long>();
}
