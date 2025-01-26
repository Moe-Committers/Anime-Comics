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
        var pages = await _db.pages
            .Where(p => p.ChapterId == request.ChapterId)
            .ToListAsync(ct);

        if (!pages.Any())
            throw new NotFoundExceptions("No pages found to update!");

        foreach (var order in request.NewOrder)
        {
            var page = pages.FirstOrDefault(p => p.Id == order.PageId);
            if (page != null)
            {
                page.PageNumber = order.NewPageNumber;
                page.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _db.SaveChangesAsync(ct);

        var updatedPages = await _db.pages
            .Where(p => p.ChapterId == request.ChapterId)
            .OrderBy(p => p.PageNumber)
            .ToListAsync(ct);

        return updatedPages.Adapt<List<PageDto>>();
    }
}