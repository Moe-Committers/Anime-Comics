using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Utils.DTOs.Favourite;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Favourite.Queries.GetFavourites;

public class GetUserFavouritesQueryHandler : IRequestHandler<GetUserFavouritesQuery, List<FavouriteBookDto>>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public GetUserFavouritesQueryHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<List<FavouriteBookDto>> Handle(GetUserFavouritesQuery request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return await _db.favourites
            .Include(f => f.Book)
                .ThenInclude(b => b.Users)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FavouriteBookDto
            {
                Id = f.Book.id,
                Title = f.Book.Title,
                Author = f.Book.Author,
                Description = f.Book.Description,
                ImageUrl = f.Book.ImageUrl,
                FavCount = f.Book.Fav,
                CreatorName = f.Book.Users.Name,
                AddedToFavouriteAt = f.CreatedAt
            })
            .ToListAsync(ct);
    }
}