using anime_comics.DB;
using anime_comics.Utils.DTOs.Chapters;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Chapter.Queries.GetChapter;

public class GetChapterQueryHandler : IRequestHandler<GetChapterQuery, ChapterDto>
{
    private readonly database _db;

    public GetChapterQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ChapterDto> Handle(GetChapterQuery request, CancellationToken ct)
    {
        var query = _db.Chapters.AsQueryable();

        var chapter = await query.FirstOrDefaultAsync(c => c.Id == request.ChapterId, ct);

        if (chapter == null)
            throw new NotFoundExceptions("Chapter not found");

        return chapter.Adapt<ChapterDto>();
    }
}