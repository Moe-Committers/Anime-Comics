using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Delete;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IImageService _imageService;

    public DeleteBookCommandHandler(database db, IHttpContextAccessor httpContext, IImageService imageService)
    {
        _db = db;
        _httpContext = httpContext;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var book = await _db.books.FirstOrDefaultAsync(b =>
            b.Id == request.Id && b.UserId == userId, ct);
        if (book == null) return false;
        if (!string.IsNullOrEmpty(book.ImageUrl)) _imageService.DeleteImage(book.ImageUrl);
        _db.books.Remove(book);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}