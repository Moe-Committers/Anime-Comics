using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetPopularBook;

public record GetPopularBookQuery : IRequest<List<BookDetailDto>> {
    public int Limited = 10;
};