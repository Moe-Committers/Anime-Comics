
using anime_comics.Utils.DTOs.Comment;
using MediatR;

namespace anime_comics.Features.Comment.Queries.GetComments;

public record GetBookCommentsQuery(long BookId) : IRequest<List<CommentDto>>;