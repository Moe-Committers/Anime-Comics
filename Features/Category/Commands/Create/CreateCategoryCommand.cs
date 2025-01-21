using anime_comics.Utils.Enum;
using MediatR;

namespace anime_comics.Features.Category.Commands.Create;

public record CreateCategoryCommand : IRequest<long> {
    public string Name {get; set;}
    public string Icon {get; set;}
    public int Order {get; set;}
};