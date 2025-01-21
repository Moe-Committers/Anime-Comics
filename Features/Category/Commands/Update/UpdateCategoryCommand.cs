using System.Text.Json.Serialization;
using anime_comics.Utils.Enum;
using MediatR;

namespace anime_comics.Features.Category.Commands.Update;

public record UpdateCategoryCommand : IRequest<bool> {
    [JsonIgnore]
    public long Id {get; set;}
    public string? Name {get; set;}
    public Status? status {get; set;}
    public string? Icon {get; set;}
    public int? Order {get; set;}
};