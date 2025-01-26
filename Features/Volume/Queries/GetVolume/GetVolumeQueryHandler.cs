using anime_comics.DB;
using anime_comics.Utils.DTOs.Volumes;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Volume.Queries.GetVolume;

public class GetVolumeQueryHandler : IRequestHandler<GetVolumeQuery, VolumeDto>
{
    private readonly database _db;

    public GetVolumeQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<VolumeDto> Handle(GetVolumeQuery request, CancellationToken ct)
    {
        var volume = await _db.volumes
            .FirstOrDefaultAsync(v => v.Id == request.VolumeId, ct);

        if (volume == null)
            throw new NotFoundExceptions("Volume not found");

        return volume.Adapt<VolumeDto>();
    }
}