using MediatR;

namespace anime_comics.Features.Category.Commands.Delete;

public record DeleteCategoryCommand(long Id) : IRequest<bool>;