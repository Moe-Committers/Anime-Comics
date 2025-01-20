using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Comment.Commands.Create;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, long>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public CreateCommentCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<long> Handle(CreateCommentCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!await _db.books.AnyAsync(b => b.id == request.BookId, ct))
            throw new NotFoundExceptions("Book not found");

        if (request.ParentId.HasValue)
        {
            var parentComment = await _db.comments.FindAsync(request.ParentId);
            if (parentComment == null || parentComment.BookId != request.BookId)
                throw new BadRequestExceptions("Invalid parent comment");
        }

        var comment = new Comments
        {
            BookId = request.BookId,
            UserId = userId,
            parentId = request.ParentId,
            Content = request.Content,
            CreatedAt = DateTime.Now
        };

        _db.comments.Add(comment);
        await _db.SaveChangesAsync(ct);

        return comment.id;
    }
}