using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Delete;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public DeleteBookCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _db.books.FirstOrDefaultAsync(b => 
            b.id == request.Id && b.UserId == userId, ct);

        if (book == null) return false;

        _db.books.Remove(book);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}