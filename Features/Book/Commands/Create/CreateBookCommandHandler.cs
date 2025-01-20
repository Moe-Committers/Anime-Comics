using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Create;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, long>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public CreateBookCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<long> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var categories = await _db.categories
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(ct);

        var book = new Books
        {
            Title = request.Title,
            Description = request.Description,
            Author = request.Author,
            ImageUrl = request.ImageUrl,
            UserId = userId,
            Categories = categories,
            CreatedAt = DateTime.Now
        };

        _db.books.Add(book);
        await _db.SaveChangesAsync(ct);

        return book.id;
    }
}
