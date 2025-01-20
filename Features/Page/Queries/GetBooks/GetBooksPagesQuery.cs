using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Queries.GetBooks;

public record GetPageQuery(long Id) : IRequest<PageDto>;