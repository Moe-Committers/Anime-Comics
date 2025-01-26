using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Chapters;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Chapter.Commands.AddChapter;

public class AddChapterCommandHandler : IRequestHandler<AddChapterCommand, ChapterDto>
{
    private readonly database _db;

    public AddChapterCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<ChapterDto> Handle(AddChapterCommand request, CancellationToken ct)
    {
        var volume = await _db.volumes
            .FirstOrDefaultAsync(v => v.Id == request.VolumeId, ct);

        if (volume == null)
            throw new NotFoundExceptions("Volume not found");

        var existingChapter = await _db.Chapters
            .FirstOrDefaultAsync(c => c.VolumeId == request.VolumeId &&
                                    c.ChapNo == request.ChapNo, ct);

        if (existingChapter != null)
            throw new BadRequestExceptions($"Chapter {request.ChapNo} already exists in this volume");

        var chapter = new Chapters
        {
            VolumeId = request.VolumeId,
            ChapNo = request.ChapNo,
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Description = request.Description,
            PageCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        _db.Chapters.Add(chapter);
        await _db.SaveChangesAsync(ct);

        return chapter.Adapt<ChapterDto>();
    }
}