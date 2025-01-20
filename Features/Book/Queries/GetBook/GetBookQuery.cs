using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetBook;

public record GetBookQuery(long Id) : IRequest<BookDetailDto>;