using anime_comics.DB;
using anime_comics.Utils.DTOs.Comment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Comment.Queries.GetComments;

public class GetBookCommentsQueryHandler : IRequestHandler<GetBookCommentsQuery, List<CommentDto>>
{
    private readonly database _db;

    public GetBookCommentsQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<List<CommentDto>> Handle(GetBookCommentsQuery request, CancellationToken ct)
    {
        var comments = await _db.comments
            .Include(c => c.User)
            .Where(c => c.BookId == request.BookId && !c.parentId.HasValue)  // Get root comments only
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                UserId = c.UserId,
                UserName = c.User.Name,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(ct);

        foreach (var comment in comments)
        {
            comment.Replies = await GetReplies(comment.Id, ct);
        }

        return comments;
    }

    private async Task<List<CommentDto>> GetReplies(long parentId, CancellationToken ct)
    {
        return await _db.comments
            .Include(c => c.User)
            .Where(c => c.parentId == parentId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                UserId = c.UserId,
                UserName = c.User.Name,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(ct);
    }
}