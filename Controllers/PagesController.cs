using anime_comics.Features.Page.Commands.Add;
using anime_comics.Features.Page.Commands.Delete;
using anime_comics.Features.Page.Commands.Update;
using anime_comics.Features.Page.Queries.GetBook;
using anime_comics.Features.Page.Queries.GetBooks;
using anime_comics.Utils.DTOs.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<List<PageDto>>> GetBookPages(long bookId)
    {
        var query = new GetBookPagesQuery(bookId);
        var pages = await _mediator.Send(query);
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PageDto>> GetPage(long id)
    {
        var query = new GetPageQuery(id);
        var page = await _mediator.Send(query);
        return Ok(page);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<long>> AddPage(AddPageCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePage(long id, UpdatePageCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePage(long id)
    {
        var command = new DeletePageCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}