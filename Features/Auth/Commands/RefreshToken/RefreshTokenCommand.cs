using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;