using MediatR;

namespace anime_comics.Features.Comment.Commands.Create;

public record CreateCommentCommand : IRequest<long>
{
    public long BookId { get; init; }
    public long? ParentId { get; init; }
    public string Content { get; init; }
}