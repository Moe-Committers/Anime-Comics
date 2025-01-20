using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Commands.Update;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly database _db;

    public UpdateCategoryCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        if (await _db.categories.AnyAsync(c => c.Name == request.Name && c.Id != request.Id, ct))
        {
            throw new BadRequestExceptions("Category name already exists");
        }

        var category = await _db.categories.FindAsync(request.Id);
        if (category == null) return false;

        category.Name = request.Name;
        await _db.SaveChangesAsync(ct);

        return true;
    }
}