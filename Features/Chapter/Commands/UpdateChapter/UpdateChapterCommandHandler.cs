using anime_comics.DB;
using anime_comics.Utils.DTOs.Chapters;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Chapter.Commands.UpdateChapter;

public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, ChapterDto>
{
    private readonly database _db;

    public UpdateChapterCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<ChapterDto> Handle(UpdateChapterCommand request, CancellationToken ct)
    {
        var chapter = await _db.Chapters
            .FirstOrDefaultAsync(c => c.Id == request.ChapterId &&
                                    c.VolumeId == request.VolumeId, ct);

        if (chapter == null)
            throw new NotFoundExceptions("Chapter not found");

        if (request.ChapNo.HasValue && request.ChapNo != chapter.ChapNo)
        {
            var existingChapter = await _db.Chapters
                .FirstOrDefaultAsync(c => c.VolumeId == request.VolumeId &&
                                        c.ChapNo == request.ChapNo, ct);

            if (existingChapter != null)
                throw new BadRequestExceptions($"Chapter number {request.ChapNo} already exists in this volume");

            chapter.ChapNo = request.ChapNo.Value;
        }

        chapter.Title = request.Title ?? chapter.Title;
        chapter.ReleaseDate = request.ReleaseDate ?? chapter.ReleaseDate;
        chapter.Description = request.Description ?? chapter.Description;
        chapter.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return chapter.Adapt<ChapterDto>();
    }
}