using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetBook;

public record GetBookPagesQuery(long BookId) : IRequest<List<PageDto>>;