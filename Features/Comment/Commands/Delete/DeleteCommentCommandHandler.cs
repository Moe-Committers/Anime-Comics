using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Comment.Commands.Delete;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public DeleteCommentCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var comment = await _db.comments.FirstOrDefaultAsync(c =>
            c.Id == request.Id && c.UserId == userId, ct);

        if (comment == null) return false;

        var replies = await _db.comments
            .Where(c => c.parentId == comment.Id)
            .ToListAsync(ct);

        _db.comments.RemoveRange(replies);
        _db.comments.Remove(comment);

        await _db.SaveChangesAsync(ct);
        return true;
    }
}