using anime_comics.Utils.DTOs.Chapters;
using MediatR;

namespace anime_comics.Features.Chapter.Queries.GetVolumeChapters;

public record GetVolumeChaptersQuery : IRequest<ApiResponse<List<ChapterDto>>>
{
    public string? Search {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
    public long VolumeId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}