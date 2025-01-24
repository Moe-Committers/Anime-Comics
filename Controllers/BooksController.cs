using anime_comics.Features.Book.Commands.Create;
using anime_comics.Features.Book.Commands.Delete;
using anime_comics.Features.Book.Commands.Publish;
using anime_comics.Features.Book.Commands.Update;
using anime_comics.Features.Book.Queries.GetBook;
using anime_comics.Features.Book.Queries.GetBooks;
using anime_comics.Features.Book.Queries.GetPopularBook;
using anime_comics.Features.Book.Queries.GetShowcase;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.DTOs.ShowcaseResponse;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.ResponseHelper;
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

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("publish/{id}")]
    public async Task<ActionResult<bool>> Publishing(long id, PublishBookCommand command)
    {
        var updateCommand = command with { Id = id };
        var success = await _mediator.Send(updateCommand);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("published")]
    public async Task<ActionResult<ApiResponse<PageResponse<BookDto>>>> GetPublishedBooks([FromQuery] GetBooksQuery query)
    {
        var newQuery = query with { pulished = true };
        var books = await _mediator.Send(newQuery);
        return Ok(ResHelper.Success(books));
    }

    [HttpGet("Popular-updates")]
    public async Task<ActionResult<List<BookDetailDto>>> GetPopularUpdates([FromQuery] GetPopularBookQuery query){
        var data = await _mediator.Send(query);
        return Ok(data);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpGet]
    public async Task<ActionResult<PageResponse<BookDto>>> GetBooks([FromQuery] GetBooksQuery query)
    {
        var newQuery = query with { pulished = false };
        var books = await _mediator.Send(newQuery);
        return Ok(books);
    }

    [HttpGet("published/{id}")]
    public async Task<ActionResult<BookDetailDto>> GetPublishedBook(long id)
    {
        var query = new GetBookQuery();
        var newQuery = query with { Id = id, published = true };
        var book = await _mediator.Send(newQuery);
        return Ok(book);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailDto>> GetBook(long id)
    {
        var query = new GetBookQuery();
        var newQuery = query with { Id = id, published = false };
        var book = await _mediator.Send(newQuery);
        return Ok(book);
    }

    [HttpGet("showcase")]
    public async Task<ActionResult<ShowcaseResponse>> GetShowcase()
    {
        var query = new GetShowcaseQuery();
        var showcase = await _mediator.Send(query);
        return Ok(showcase);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost]
    public async Task<ActionResult<long>> CreateBook([FromForm] CreateBookCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(long id, [FromForm] UpdateBook req)
    {
        var command = new UpdateBookCommand {
            Id = id,
            Title = req.Title,
            Description = req.Description,
            Author = req.Author,
            ImageUrl = req.ImageUrl,
            CategoryIds = req.CategoryIds
        };
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBook(long id)
    {
        var command = new DeleteBookCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}