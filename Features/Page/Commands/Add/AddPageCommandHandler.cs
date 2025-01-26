using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Add;

public class AddPagesCommandHandler : IRequestHandler<AddPagesCommand, List<PageDto>>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public AddPagesCommandHandler(database db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<List<PageDto>> Handle(AddPagesCommand request, CancellationToken ct)
    {
        var chapter = await _db.Chapters
            .Include(c => c.Volume)
            .Include(c => c.Pages)
            .FirstOrDefaultAsync(c => c.Id == request.ChapterId, ct);

        if (chapter == null)
            throw new NotFoundExceptions("Chapter not found");

        var startingPageNumber = chapter.Pages.Any() 
            ? chapter.Pages.Max(p => p.PageNumber) + 1 
            : 1;

        var newPages = new List<Pages>();

        for (int i = 0; i < request.Images.Count; i++)
        {
            var image = request.Images[i];
            var pageNumber = startingPageNumber + i;

            var imageUrl = await _imageService.UploadImage(
                image, 
                $"book-{chapter.Volume.BookId}/volume-{chapter.Volume.VolumeNo}/chapter-{chapter.ChapNo}/page-{pageNumber}"
            );

            var page = new Pages
            {
                ChapterId = request.ChapterId,
                PageNumber = pageNumber,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            newPages.Add(page);
        }

        _db.pages.AddRange(newPages);

        chapter.UpdatePageCount();
        chapter.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync(ct);

        return newPages.Adapt<List<PageDto>>();
    }
}
