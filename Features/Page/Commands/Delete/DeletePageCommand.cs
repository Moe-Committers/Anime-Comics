using MediatR;

namespace anime_comics.Features.Page.Commands.Delete;

public record DeletePageCommand(long Id) : IRequest<bool>;