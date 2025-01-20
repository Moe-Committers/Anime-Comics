using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetBook;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDetailDto>
{
    private readonly database _db;

    public GetBookQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<BookDetailDto> Handle(GetBookQuery request, CancellationToken ct)
    {
        var book = await _db.books
            .Include(b => b.Categories)
            .Include(b => b.Users)
            .Include(b => b.Pages)
            .FirstOrDefaultAsync(b => b.id == request.Id, ct);

        if (book == null)
            throw new NotFoundExceptions("Book not found");

        return book.Adapt<BookDetailDto>();
    }
}