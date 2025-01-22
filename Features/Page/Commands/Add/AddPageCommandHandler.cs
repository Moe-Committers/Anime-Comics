using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Add;

public class AddPageCommandHandler : IRequestHandler<AddPageCommand, List<long>>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IImageService _imageService;

    public AddPageCommandHandler(database db, IHttpContextAccessor httpContext, IImageService imageService)
    {
        _db = db;
        _httpContext = httpContext;
        _imageService = imageService;
    }

    public async Task<List<long>> Handle(AddPageCommand request, CancellationToken ct)
    {
        var book = await _db.books
            .Include(b => b.Pages)
            .FirstOrDefaultAsync(b => b.Id == request.BookId, ct);

        if (book == null)
            throw new NotFoundExceptions("Book not found or you don't have permission");

        var startingPageNumber = book.Pages.Any() ? book.Pages.Max(p => p.PageNumber) + 1 : 1;

        var newPages = new List<Pages>();
        var pageIds = new List<long>();

        for (int i = 0; i < request.ImageUrl.Count; i++)
        {
            var image = request.ImageUrl[i];
            var pageNumber = startingPageNumber + i;

            var imageUrl = await _imageService.UploadImage(image, $"book-{request.BookId}/page-{pageNumber}");

            var page = new Pages
            {
                BookId = request.BookId,
                PageNumber = pageNumber,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.Now
            };

            newPages.Add(page);
        }

        _db.pages.AddRange(newPages);
        await _db.SaveChangesAsync(ct);

        return newPages.Select(p => p.Id).ToList();
    }
}
