using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Commands.Register;

public record RegisterCommand(string Name, int Age, string Email, string Password) : IRequest<AuthResponse>;