using MediatR;

namespace anime_comics.Features.Category.Commands.Update;

public record UpdateCategoryCommand(long Id, string Name) : IRequest<bool>;