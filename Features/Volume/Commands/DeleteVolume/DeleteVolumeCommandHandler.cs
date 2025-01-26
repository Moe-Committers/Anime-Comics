using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Volume.Commands.DeleteVolume;

public class DeleteVolumeCommandHandler : IRequestHandler<DeleteVolumeCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public DeleteVolumeCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeleteVolumeCommand request, CancellationToken ct)
    {
        var volume = await _db.volumes
            .Include(v => v.Chapters)
                .ThenInclude(c => c.Pages)
            .FirstOrDefaultAsync(v => v.Id == request.VolumeId && 
                                    v.BookId == request.BookId, ct);

        if (volume == null)
            return false;

        if (!string.IsNullOrEmpty(volume.CoverImg))
            _imageService.DeleteImage(volume.CoverImg);

        foreach (var chapter in volume.Chapters)
        {
            foreach (var page in chapter.Pages)
            {
                _imageService.DeleteImage(page.ImageUrl);
            }
        }

        _db.volumes.Remove(volume);
        await _db.SaveChangesAsync(ct);

        return true;
    }
}