using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Delete;

public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public DeletePageCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeletePageCommand request, CancellationToken ct)
    {
        var pages = await _db.pages
            .Include(p => p.Book)
            .Where(p => p.BookId == request.BookId &&
                       request.PageIds.Contains(p.Id))
            .ToListAsync(ct);

        if (!pages.Any())
            return false;

        foreach (var page in pages)
        {
            _imageService.DeleteImage(page.ImageUrl);
        }

        _db.pages.RemoveRange(pages);
        await _db.SaveChangesAsync(ct);

        return true;
    }
}