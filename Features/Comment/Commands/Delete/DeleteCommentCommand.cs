using MediatR;

namespace anime_comics.Features.Comment.Commands.Delete;

public record DeleteCommentCommand(long Id) : IRequest<bool>;