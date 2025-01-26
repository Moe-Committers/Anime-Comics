using anime_comics.Utils.DTOs.Chapters;
using MediatR;

namespace anime_comics.Features.Chapter.Commands.UpdateChapter;

public record UpdateChapterCommand : IRequest<ChapterDto>
{
    public long VolumeId { get; init; }
    public long ChapterId { get; init; }
    public int? ChapNo { get; init; }
    public string? Title { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public string? Description { get; init; }
}