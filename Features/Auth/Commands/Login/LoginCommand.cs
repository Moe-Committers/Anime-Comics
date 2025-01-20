using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;