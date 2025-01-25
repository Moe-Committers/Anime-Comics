using MediatR;

namespace anime_comics.Features.SiteSetting.Commands.Update;

public record UpdateSiteSetting : IRequest<anime_comics.Models.SiteSetting>{
    public long Id {get; set;}
    public string Title {get; set;}
    public IFormFile Logo {get; set;}
}