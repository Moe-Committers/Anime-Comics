using MediatR;

namespace anime_comics.Features.Chapter.Commands.DeleteChapter;

public record DeleteChapterCommand : IRequest<bool>
{
    public long VolumeId { get; init; }
    public long ChapterId { get; init; }
}