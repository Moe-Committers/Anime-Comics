using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Commands.Update;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public UpdateCategoryCommandHandler(database db , IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        if (await _db.categories.AnyAsync(c => c.Name == request.Name && c.Id != request.Id, ct))
        {
            throw new BadRequestExceptions("Category name already exists");
        }

        var category = await _db.categories.FindAsync(request.Id);
        if (category == null) return false;

        category.Name = request.Name ?? category.Name;
        category.status = request.status ?? category.status;
        if(request.Icon != null){
            if(!string.IsNullOrEmpty(category.Icon)){
                _imageService.DeleteImage(category.Icon);
            }
            category.Icon = await _imageService.UploadImage(request.Icon ,"category");
        }
        category.Order = request.Order ?? category.Order;

        await _db.SaveChangesAsync(ct);

        return true;
    }
}