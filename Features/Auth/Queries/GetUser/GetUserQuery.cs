using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Queries.GetUser;

public record GetUserQuery(long UserId) : IRequest<UserDto>;