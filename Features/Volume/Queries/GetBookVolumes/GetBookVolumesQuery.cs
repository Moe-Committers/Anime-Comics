using anime_comics.Utils.DTOs.Volumes;
using MediatR;

namespace anime_comics.Features.Volume.Queries.GetBookVolumes;

public record GetBookVolumesQuery : IRequest<ApiResponse<List<VolumeDto>>>
{
    public string? Search {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
    public long BookId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}