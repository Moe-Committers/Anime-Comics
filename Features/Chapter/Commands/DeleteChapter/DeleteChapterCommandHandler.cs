using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Chapter.Commands.DeleteChapter;

public class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public DeleteChapterCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeleteChapterCommand request, CancellationToken ct)
    {
        var chapter = await _db.Chapters
            .Include(c => c.Pages)
            .FirstOrDefaultAsync(c => c.Id == request.ChapterId && 
                                    c.VolumeId == request.VolumeId, ct);

        if (chapter == null)
            return false;

        foreach (var page in chapter.Pages)
        {
            _imageService.DeleteImage(page.ImageUrl);
        }

        _db.Chapters.Remove(chapter);
        await _db.SaveChangesAsync(ct);

        return true;
    }
}