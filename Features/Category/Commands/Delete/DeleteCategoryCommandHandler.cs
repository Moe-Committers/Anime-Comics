using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Commands.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public DeleteCategoryCommandHandler(database db , IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var category = await _db.categories
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category == null) return false;

        if (category.Books.Any())
        {
            throw new BadRequestExceptions("Cannot delete category with associated books");
        }
        if(!string.IsNullOrEmpty(category.Icon)){
            _imageService.DeleteImage(category.Icon);
        }

        _db.categories.Remove(category);
        await _db.SaveChangesAsync(ct);

        return true;
    }
}