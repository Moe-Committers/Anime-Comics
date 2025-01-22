using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Helpers.Exceptions;
using DocumentFormat.OpenXml.Wordprocessing;
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
        var bookQuery = _db.books
            .Include(b => b.Categories)
            .Include(b => b.Users)
            .Where(b => b.Id == request.Id);

        if(request.published){
            bookQuery = bookQuery.Where(b => b.Published_at != null);
        }

        var book = await bookQuery.Select(b => new BookDetailDto
        {
            Id = b.Id,
            Title = b.Title,
            Description = b.Description,
            Author = b.Author,
            Published_at = b.Published_at,
            ImageUrl = b.ImageUrl,
            UserName = b.Users.Name,
            Fav = b.Fav,
            Categories = b.Categories.Select(c => new Cate {
                Id = c.Id,
                Name = c.Name,
                Order = c.Order
            }).ToList()
        }).FirstOrDefaultAsync(ct);

        if (book == null)
            throw new NotFoundExceptions("Book not found");

        return book.Adapt<BookDetailDto>();
    }
}