using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Favourite.Queries.GetFavourite;

public class CheckFavouriteStatusQueryHandler : IRequestHandler<CheckFavouriteStatusQuery, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public CheckFavouriteStatusQueryHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(CheckFavouriteStatusQuery request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return await _db.favourites.AnyAsync(f =>
            f.BookId == request.BookId && f.UserId == userId, ct);
    }
}