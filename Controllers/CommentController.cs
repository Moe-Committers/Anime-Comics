using anime_comics.Features.Comment.Commands.Create;
using anime_comics.Features.Comment.Commands.Delete;
using anime_comics.Features.Comment.Commands.Update;
using anime_comics.Features.Comment.Queries.GetComment;
using anime_comics.Features.Comment.Queries.GetComments;
using anime_comics.Utils.DTOs.Comment;
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
    public async Task<ActionResult<List<CommentDto>>> GetBookComments(long bookId)
    {
        var query = new GetBookCommentsQuery(bookId);
        var comments = await _mediator.Send(query);
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(long id)
    {
        var query = new GetCommentQuery(id);
        var comment = await _mediator.Send(query);
        return Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<long>> CreateComment(CreateCommentCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(long id, UpdateCommentCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(long id)
    {
        var command = new DeleteCommentCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}