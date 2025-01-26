using anime_comics.Utils.DTOs.Volumes;
using MediatR;

namespace anime_comics.Features.Volume.Commands.AddVolume;

public record AddVolumeCommand : IRequest<VolumeDto>
{
    public long BookId { get; init; }
    public int VolumeNo { get; init; }
    public string Title { get; init; }
    public IFormFile? CoverImg { get; init; }
    public DateTime? ReleaseDate { get; init; }
    public string Description { get; init; }
}