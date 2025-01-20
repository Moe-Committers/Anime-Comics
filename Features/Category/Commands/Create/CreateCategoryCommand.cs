using MediatR;

namespace anime_comics.Features.Category.Commands.Create;

public record CreateCategoryCommand(string Name) : IRequest<long>;