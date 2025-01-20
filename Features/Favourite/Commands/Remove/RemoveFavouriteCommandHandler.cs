using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Favourite.Commands.Remove;

public class RemoveFavouriteCommandHandler : IRequestHandler<RemoveFavouriteCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public RemoveFavouriteCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(RemoveFavouriteCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var favourite = await _db.favourites
            .FirstOrDefaultAsync(f => f.BookId == request.BookId && f.UserId == userId, ct);

        if (favourite == null)
            return false;

        _db.favourites.Remove(favourite);

        var book = await _db.books.FindAsync(request.BookId);
        if (book != null && book.Fav > 0)
            book.Fav--;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}