using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Update;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IImageService _imageService;

    public UpdateBookCommandHandler(database db, IHttpContextAccessor httpContext , IImageService imageService)
    {
        _db = db;
        _httpContext = httpContext;
        _imageService = imageService;
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
        if(request.ImageUrl != null){
            if(!string.IsNullOrEmpty(book.ImageUrl)){
                _imageService.DeleteImage(book.ImageUrl);
            }
            book.ImageUrl = await _imageService.UploadImage(request.ImageUrl,"book-cover");
        }
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
