using System.Security.Claims;
using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Delete;

public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public DeletePageCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(DeletePageCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var page = await _db.pages
            .Include(p => p.Book)
            .FirstOrDefaultAsync(p => p.id == request.Id && p.Book.UserId == userId, ct);

        if (page == null) return false;

        _db.pages.Remove(page);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}