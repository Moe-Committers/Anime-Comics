using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.Update;

public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, bool>
{
    private readonly database _db;
    private readonly IHttpContextAccessor _httpContext;

    public UpdatePageCommandHandler(database db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> Handle(UpdatePageCommand request, CancellationToken ct)
    {
        var userId = long.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var page = await _db.pages
            .Include(p => p.Book)
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.Book.UserId == userId, ct);

        if (page == null) return false;

        if (await _db.pages.AnyAsync(p => 
            p.BookId == request.BookId && 
            p.PageNumber == request.PageNumber && 
            p.Id != request.Id, ct))
        {
            throw new BadRequestExceptions($"Page {request.PageNumber} already exists");
        }

        page.PageNumber = request.PageNumber;
        page.ImageUrl = request.ImageUrl;
        page.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}