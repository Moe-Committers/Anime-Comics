using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Category.Commands.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly database _db;

    public DeleteCategoryCommandHandler(database db)
    {
        _db = db;
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

        _db.categories.Remove(category);
        await _db.SaveChangesAsync(ct);

        return true;
    }
}