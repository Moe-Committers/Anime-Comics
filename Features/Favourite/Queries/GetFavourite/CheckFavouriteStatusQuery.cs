using MediatR;

namespace anime_comics.Features.Favourite.Queries.GetFavourite;

public record CheckFavouriteStatusQuery(long BookId) : IRequest<bool>;