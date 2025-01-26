using anime_comics.DB;
using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Delete;

public class DeletePagesCommandHandler : IRequestHandler<DeletePagesCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public DeletePagesCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeletePagesCommand request, CancellationToken ct)
    {
        var pages = await _db.pages
            .Include(p => p.Chapter)
            .Where(p => p.ChapterId == request.ChapterId &&
                       request.PageIds.Contains(p.Id))
            .ToListAsync(ct);

        if (!pages.Any())
            return false;

        var chapter = pages.First().Chapter;

        foreach (var page in pages)
        {
            _imageService.DeleteImage(page.ImageUrl);
        }

        _db.pages.RemoveRange(pages);

        chapter.UpdatePageCount();
        chapter.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        await ReorderRemainingPages(chapter.Id, ct);

        return true;
    }

    private async Task ReorderRemainingPages(long chapterId, CancellationToken ct)
    {
        var remainingPages = await _db.pages
            .Where(p => p.ChapterId == chapterId)
            .OrderBy(p => p.PageNumber)
            .ToListAsync(ct);

        for (int i = 0; i < remainingPages.Count; i++)
        {
            remainingPages[i].PageNumber = i + 1;
            remainingPages[i].UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(ct);
    }
}
