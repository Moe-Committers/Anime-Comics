using anime_comics.Utils.DTOs.Chapters;
using MediatR;

namespace anime_comics.Features.Chapter.Queries.GetChapter;

public record GetChapterQuery : IRequest<ChapterDto>
{
    public long ChapterId { get; init; }
}
