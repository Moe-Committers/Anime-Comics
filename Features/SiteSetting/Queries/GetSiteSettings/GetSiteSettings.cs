using MediatR;

namespace anime_comics.Features.SiteSetting.Queries.GetSiteSettings;

public record GetSiteSettings : IRequest<List<Models.SiteSetting>>;