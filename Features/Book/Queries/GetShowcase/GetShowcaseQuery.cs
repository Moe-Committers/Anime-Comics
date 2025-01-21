using anime_comics.Utils.DTOs.ShowcaseResponse;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetShowcase;

public record GetShowcaseQuery : IRequest<ShowcaseResponse>;