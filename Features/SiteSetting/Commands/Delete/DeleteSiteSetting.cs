using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.SiteSetting.Commands.Delete;

public record DeleteSiteSetting : IRequest<bool> {
    [JsonIgnore]
    public long Id {get; set;}
}