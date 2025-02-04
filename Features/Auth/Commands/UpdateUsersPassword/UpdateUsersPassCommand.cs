
using System.Text.Json.Serialization;
using MediatR;

namespace anime_comics.Features.Auth.Commands.UpdateUsersPassword;

public record UpdateUsersPassCommand : IRequest<bool> {
    [JsonIgnore]
    public long Id {get; set;}
    public string NewPassword {get; set;}
    public string ConfirmPassword {get; set;}
};