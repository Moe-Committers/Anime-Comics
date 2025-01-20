using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Comment.Commands.Update;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public UpdateCommentCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(UpdateCommentCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var comment = await _db.comments.FirstOrDefaultAsync(c =>
            c.id == request.Id && c.UserId == userId, ct);

        if (comment == null) return false;

        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
