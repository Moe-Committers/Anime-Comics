using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Commands.ToggleActive;

public record ToggleActiveCommand : IRequest<UserDto>{
    public long Id {get; set;}
    public bool Toggle {get; set;}
};