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
        var page = await _db.pages.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (page == null) throw new NotFoundExceptions("Page not founded!");

        page.PageNumber = request.PageNumber ?? page.PageNumber;
        if (request.ImageUrl != null)
        {
            if (!string.IsNullOrEmpty(page.ImageUrl))
            {
                _imageService.DeleteImage(page.ImageUrl);
            }
            var imagePath = $"book-{page.BookId}/page-{page.PageNumber}";
            page.ImageUrl = await _imageService.UploadImage(request.ImageUrl, imagePath);
        }
        page.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync(ct);
        return page.Adapt<PageDto>();
    }
}