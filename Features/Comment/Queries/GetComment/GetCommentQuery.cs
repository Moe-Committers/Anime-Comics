using anime_comics.Utils.DTOs.Comment;
using MediatR;

namespace anime_comics.Features.Comment.Queries.GetComment;

public record GetCommentQuery(long Id) : IRequest<CommentDto>;