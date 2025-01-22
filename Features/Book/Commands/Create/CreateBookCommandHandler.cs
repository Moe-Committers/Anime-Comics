using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Create;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, long>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IImageService _imageService;

    public CreateBookCommandHandler(database db, IHttpContextAccessor httpContext , IImageService imageService)
    {
        _db = db;
        _httpContext = httpContext;
        _imageService = imageService;
    }

    public async Task<long> Handle(CreateBookCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        
        var user = await _db.users.FindAsync(userId);
        if (user == null)
            throw new NotFoundExceptions("User not found");

        var categories = await _db.categories
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(ct);

        var book = new Books
        {
            Title = request.Title,
            Description = request.Description,
            Author = request.Author,
            ImageUrl = await _imageService.UploadImage(request.ImageUrl,"book-cover"),
            UserId = userId,
            Users = user,
            Categories = categories,
            CreatedAt = DateTime.UtcNow
        };

        _db.books.Add(book);
        await _db.SaveChangesAsync(ct);

        return book.Id;
    }
}
