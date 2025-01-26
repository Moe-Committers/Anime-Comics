using anime_comics.DB;
using anime_comics.Utils.DTOs.Volumes;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Volume.Commands.AddVolume;

public class AddVolumeCommandHandler : IRequestHandler<AddVolumeCommand, VolumeDto>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public AddVolumeCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<VolumeDto> Handle(AddVolumeCommand request, CancellationToken ct)
    {
        var book = await _db.books
            .FirstOrDefaultAsync(b => b.Id == request.BookId, ct);

        if (book == null)
            throw new NotFoundExceptions("Book not found");

        var existingVolume = await _db.volumes
            .FirstOrDefaultAsync(v => v.BookId == request.BookId &&
                                    v.VolumeNo == request.VolumeNo, ct);

        if (existingVolume != null)
            throw new BadRequestExceptions($"Volume {request.VolumeNo} already exists for this book");

        var volume = new Models.Volumes
        {
            BookId = request.BookId,
            VolumeNo = request.VolumeNo,
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        if (request.CoverImg != null)
        {
            volume.CoverImg = await _imageService.UploadImage(
                request.CoverImg,
                $"book-{request.BookId}/volume-{request.VolumeNo}"
            );
        }

        _db.volumes.Add(volume);
        await _db.SaveChangesAsync(ct);

        return volume.Adapt<VolumeDto>();
    }
}