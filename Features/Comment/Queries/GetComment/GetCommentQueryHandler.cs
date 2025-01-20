using anime_comics.DB;
using anime_comics.Utils.DTOs.Comment;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Comment.Queries.GetComment;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, CommentDto>
{
    private readonly database _db;

    public GetCommentQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<CommentDto> Handle(GetCommentQuery request, CancellationToken ct)
    {
        var comment = await _db.comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.id == request.Id, ct);

        if (comment == null)
            throw new NotFoundExceptions("Comment not found");

        var commentDto = new CommentDto
        {
            Id = comment.id,
            Content = comment.Content,
            UserId = comment.UserId,
            UserName = comment.User.Name,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };

        if (!comment.parentId.HasValue)
        {
            commentDto.Replies = await _db.comments
                .Include(c => c.User)
                .Where(c => c.parentId == comment.id)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.id,
                    Content = c.Content,
                    UserId = c.UserId,
                    UserName = c.User.Name,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync(ct);
        }

        return commentDto;
    }
}