using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.SiteSetting.Queries.GetSiteSetting;

public record GetSiteSetting : IRequest<anime_comics.Models.SiteSetting>{
    [JsonIgnore]
    public long Id {get; set;} 
};