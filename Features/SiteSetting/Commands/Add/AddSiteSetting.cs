using MediatR;

namespace anime_comics.Features.SiteSetting.Commands.Add;

public record AddSiteSetting : IRequest<Models.SiteSetting>{
    public string Title {get; set;}
    public IFormFile Logo {get; set;}
};