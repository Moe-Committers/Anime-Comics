using anime_comics.Utils.DTOs.Volumes;
using MediatR;

namespace anime_comics.Features.Volume.Queries.GetVolume;

public record GetVolumeQuery : IRequest<VolumeDto>
{
    public long VolumeId { get; init; }
}
