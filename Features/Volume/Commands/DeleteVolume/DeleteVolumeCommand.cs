using MediatR;

namespace anime_comics.Features.Volume.Commands.DeleteVolume;

public record DeleteVolumeCommand : IRequest<bool>
{
    public long BookId { get; init; }
    public long VolumeId { get; init; }
}