using anime_comics.DB;
using anime_comics.Utils.DTOs.Volumes;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Volume.Commands.UpdateVolume;

public class UpdateVolumeCommandHandler : IRequestHandler<UpdateVolumeCommand, VolumeDto>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public UpdateVolumeCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<VolumeDto> Handle(UpdateVolumeCommand request, CancellationToken ct)
    {
        var volume = await _db.volumes
            .FirstOrDefaultAsync(v => v.Id == request.VolumeId &&
                                    v.BookId == request.BookId, ct);

        if (volume == null)
            throw new NotFoundExceptions("Volume not found");

        if (request.VolumeNo.HasValue && request.VolumeNo != volume.VolumeNo)
        {
            var existingVolume = await _db.volumes
                .FirstOrDefaultAsync(v => v.BookId == request.BookId &&
                                        v.VolumeNo == request.VolumeNo, ct);

            if (existingVolume != null)
                throw new BadRequestExceptions($"Volume number {request.VolumeNo} already exists");

            volume.VolumeNo = request.VolumeNo.Value;
        }

        volume.Title = request.Title ?? volume.Title;
        volume.ReleaseDate = request.ReleaseDate ?? volume.ReleaseDate;
        volume.Description = request.Description ?? volume.Description;

        if (request.CoverImg != null)
        {
            if (!string.IsNullOrEmpty(volume.CoverImg))
                _imageService.DeleteImage(volume.CoverImg);

            volume.CoverImg = await _imageService.UploadImage(
                request.CoverImg,
                $"book-{request.BookId}/volume-{volume.VolumeNo}"
            );
        }

        volume.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return volume.Adapt<VolumeDto>();
    }
}