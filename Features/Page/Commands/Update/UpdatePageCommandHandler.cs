using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Update;

public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, PageDto>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public UpdatePageCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<PageDto> Handle(UpdatePageCommand request, CancellationToken ct)
    {
        var page = await _db.pages
            .Include(p => p.Chapter)
                .ThenInclude(c => c.Volume)
            .FirstOrDefaultAsync(p => p.Id == request.PageId &&
                                    p.ChapterId == request.ChapterId, ct);

        if (page == null)
            throw new NotFoundExceptions("Page not found");

        if (request.PageNumber.HasValue && request.PageNumber != page.PageNumber)
        {
            var existingPage = await _db.pages
                .FirstOrDefaultAsync(p => p.ChapterId == request.ChapterId &&
                                        p.PageNumber == request.PageNumber, ct);

            if (existingPage != null)
                throw new BadRequestExceptions($"Page number {request.PageNumber} already exists in this chapter");

            page.PageNumber = request.PageNumber.Value;
        }

        if (request.Image != null)
        {
            if (!string.IsNullOrEmpty(page.ImageUrl))
            {
                _imageService.DeleteImage(page.ImageUrl);
            }

            page.ImageUrl = await _imageService.UploadImage(
                request.Image,
                $"book-{page.Chapter.Volume.BookId}/volume-{page.Chapter.Volume.VolumeNo}/chapter-{page.Chapter.ChapNo}/page-{page.PageNumber}"
            );
        }

        page.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return page.Adapt<PageDto>();
    }
}
