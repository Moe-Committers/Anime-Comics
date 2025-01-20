using anime_comics.Features.Book.Commands.Create;
using anime_comics.Features.Book.Commands.Delete;
using anime_comics.Features.Book.Commands.Update;
using anime_comics.Features.Book.Queries.GetBook;
using anime_comics.Features.Book.Queries.GetBooks;
using anime_comics.Utils.DTOs.Books;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetBooks([FromQuery] GetBooksQuery query)
    {
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailDto>> GetBook(long id)
    {
        var query = new GetBookQuery(id);
        var book = await _mediator.Send(query);
        return Ok(book);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<long>> CreateBook(CreateBookCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(long id, UpdateBookCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(long id)
    {
        var command = new DeleteBookCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}