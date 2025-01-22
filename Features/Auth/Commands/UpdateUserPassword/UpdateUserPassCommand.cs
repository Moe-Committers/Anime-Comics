
using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.Auth.Commands.UpdateUserPassword;

public record UpdateUserPassCommand : IRequest<bool> {
    [JsonIgnore]
    public long Id {get; set;}
    public string CurrentPassword {get; set;}
    public string NewPassword {get; set;}
    public string ConfirmPassword {get; set;}
};