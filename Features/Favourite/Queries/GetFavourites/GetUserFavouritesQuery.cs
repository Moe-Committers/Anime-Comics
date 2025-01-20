using anime_comics.Utils.DTOs.Favourite;
using MediatR;

namespace anime_comics.Features.Favourite.Queries.GetFavourites;

public record GetUserFavouritesQuery : IRequest<List<FavouriteBookDto>>;