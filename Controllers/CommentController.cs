using anime_comics.Features.Comment.Commands.Create;
using anime_comics.Features.Comment.Commands.Delete;
using anime_comics.Features.Comment.Commands.Update;
using anime_comics.Features.Comment.Queries.GetComment;
using anime_comics.Features.Comment.Queries.GetComments;
using anime_comics.Utils.DTOs.Comment;
using anime_comics.Utils.Helpers.ResponseHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<ApiResponse<List<CommentDto>>>> GetBookComments(long bookId)
    {
        var query = new GetBookCommentsQuery(bookId);
        var comments = await _mediator.Send(query);
        return Ok(ResHelper.Success(comments));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CommentDto>>> GetComment(long id)
    {
        var query = new GetCommentQuery(id);
        var comment = await _mediator.Send(query);
        return Ok(ResHelper.Success(comment));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateComment(CreateCommentCommand command)
    {
        var id = await _mediator.Send(command);
        return id > 0 ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateComment(long id, UpdateCommentCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var success = await _mediator.Send(command);
        return success ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteComment(long id)
    {
        var command = new DeleteCommentCommand(id);
        var success = await _mediator.Send(command);
        return success ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }
}