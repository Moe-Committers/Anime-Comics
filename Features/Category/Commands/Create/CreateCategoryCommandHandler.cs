using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Commands.Create;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, long>
{
    private readonly database _db;
    private readonly IImageService _imageService;

    public CreateCategoryCommandHandler(database db , IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    public async Task<long> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        if (await _db.categories.AnyAsync(c => c.Name == request.Name, ct))
        {
            throw new BadRequestExceptions("Category name already exists");
        }

        var category = new Categories
        {
            Name = request.Name,
            status = Status.InActive,
            Icon = await _imageService.UploadImage(request.Icon , "category"),
            Order = request.Order
        };

        _db.categories.Add(category);
        await _db.SaveChangesAsync(ct);

        return category.Id;
    }
}