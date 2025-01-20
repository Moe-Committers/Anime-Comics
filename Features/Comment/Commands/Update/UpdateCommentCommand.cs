using MediatR;

namespace anime_comics.Features.Comment.Commands.Update;

public record UpdateCommentCommand : IRequest<bool>
{
    public long Id { get; init; }
    public string Content { get; init; }
}
