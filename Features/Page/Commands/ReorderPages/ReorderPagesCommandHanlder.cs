using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Page.Commands.ReorderPages;

public class ReorderPagesCommandHandler : IRequestHandler<ReorderPagesCommand, List<PageDto>>
{
    private readonly database _db;
    public ReorderPagesCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<List<PageDto>> Handle(ReorderPagesCommand request, CancellationToken ct)
    {
        var book = await _db.books
            .Include(b => b.Pages)
            .FirstOrDefaultAsync(b => b.Id == request.BookId, ct);

        if (book == null)
            throw new NotFoundExceptions("Book not founded!");

        foreach (var order in request.NewOrder)
        {
            var page = book.Pages.FirstOrDefault(p => p.Id == order.PageId);
            if (page != null)
            {
                page.PageNumber = order.NewPageNumber;
                page.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _db.SaveChangesAsync(ct);

        var updatedPages = await _db.pages
            .Where(p => p.BookId == request.BookId)
            .OrderBy(p => p.PageNumber)
            .ToListAsync(ct);

        return updatedPages.Adapt<List<PageDto>>();
    }
}