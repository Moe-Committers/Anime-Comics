using MediatR;

namespace anime_comics.Features.Favourite.Commands.Add;

public record AddFavouriteCommand(long BookId) : IRequest<bool>;