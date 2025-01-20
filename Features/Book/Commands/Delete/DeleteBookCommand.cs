using MediatR;

namespace anime_comics.Features.Book.Commands.Delete;

public record DeleteBookCommand(long Id) : IRequest<bool>;