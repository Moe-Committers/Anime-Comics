using System.Text.Json.Serialization;
using anime_comics.Utils.DTOs.Chapters;
using MediatR;

namespace anime_comics.Features.Chapter.Commands.AddChapter;

public record AddChapterCommand : IRequest<ChapterDto>
{
    [JsonIgnore]
    public long VolumeId { get; init; }
    public int ChapNo { get; init; }
    public string Title { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public string Description { get; init; }
}
