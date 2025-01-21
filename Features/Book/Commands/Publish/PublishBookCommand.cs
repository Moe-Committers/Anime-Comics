using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.Book.Commands.Publish;

public record PublishBookCommand : IRequest<bool> {
    [JsonIgnore]
    public long Id {get; set;}
    public bool publish {get; set;} = true;
};