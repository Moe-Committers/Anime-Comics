using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Update;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public UpdateBookCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _db.books
            .Include(b => b.Categories)
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.UserId == userId, ct);

        if (book == null) return false;

        book.Title = request.Title ?? book.Title;
        book.Description = request.Description ?? book.Description;
        book.Author = request.Author ?? book.Author;
        book.ImageUrl = request.ImageUrl ?? book.ImageUrl;
        book.UpdatedAt = DateTime.UtcNow;

        if (request.CategoryIds?.Any() == true)
        {
            var categories = await _db.categories
                .Where(c => request.CategoryIds.Contains(c.Id))
                .ToListAsync(ct);

            book.Categories.Clear();
            foreach (var category in categories)
            {
                book.Categories.Add(category);
            }
        }

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
