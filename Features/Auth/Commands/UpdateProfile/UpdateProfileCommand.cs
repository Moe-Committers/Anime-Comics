using System.ComponentModel.DataAnnotations.Schema;
using anime_comics.Utils.DTOs.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace anime_comics.Features.Auth.Commands.UpdateProfile;

public record UpdateProfileCommand : IRequest<UserDto> {
    [BindNever]
    [NotMapped]
    public long Id {get; set;}
    public string? Name {get; set;}
    public IFormFile? Img {get; set;}
};