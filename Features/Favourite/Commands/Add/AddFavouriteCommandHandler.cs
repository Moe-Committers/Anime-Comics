using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Favourite.Commands.Add;

public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public AddFavouriteCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(AddFavouriteCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _db.books.FindAsync(request.BookId);
        if (book == null)
            throw new NotFoundExceptions("Book not found");
            
        if (await _db.favourites.AnyAsync(f => f.BookId == request.BookId && f.UserId == userId, ct))
            return false; 

        var favourite = new Favourites
        {
            BookId = request.BookId,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        _db.favourites.Add(favourite);

        book.Fav++;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}