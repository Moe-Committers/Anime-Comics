using MediatR;

namespace anime_comics.Features.Favourite.Commands.Remove;

public record RemoveFavouriteCommand(long BookId) : IRequest<bool>;