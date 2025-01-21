using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Add;

public class AddPageCommandHandler : IRequestHandler<AddPageCommand, long>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public AddPageCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<long> Handle(AddPageCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _db.books
            .FirstOrDefaultAsync(b => b.Id == request.BookId && b.UserId == userId, ct);

        if (book == null)
            throw new NotFoundExceptions("Book not found or you don't have permission");

        if (await _db.pages.AnyAsync(p => p.BookId == request.BookId && p.PageNumber == request.PageNumber, ct))
            throw new BadRequestExceptions($"Page {request.PageNumber} already exists");

        var page = new Pages
        {
            BookId = request.BookId,
            PageNumber = request.PageNumber,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.Now
        };

        _db.pages.Add(page);
        await _db.SaveChangesAsync(ct);

        return page.Id;
    }
}
