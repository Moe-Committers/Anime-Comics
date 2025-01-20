using MediatR;

namespace anime_comics.Features.Auth.Commands.Logout;

public record LogoutCommand(long UserId, string RefreshToken) : IRequest<bool>;