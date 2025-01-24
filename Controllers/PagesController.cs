using anime_comics.Features.Page.Commands.Add;
using anime_comics.Features.Page.Commands.Delete;
using anime_comics.Features.Page.Commands.ReorderPages;
using anime_comics.Features.Page.Commands.Update;
using anime_comics.Features.Page.Queries.GetBook;
using anime_comics.Features.Page.Queries.GetBooks;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.ResponseHelper;
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
    public async Task<ActionResult<PageResponse<PageDto>>> GetBookPages(long bookId, [FromQuery] QueryingBookPages req)
    {
        var query = new GetBookPagesQuery
        {
            BookId = bookId,
            Page = req.Page,
            PageSize = req.PageSize
        };
        var pages = await _mediator.Send(query);
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PageDto>>> GetPage(long id)
    {
        var query = new GetPageQuery(id);
        var page = await _mediator.Send(query);
        return Ok(ResHelper.Success(page));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<List<long>>>> AddPage([FromForm] AddPageCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(ResHelper.Success(id));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("/reordering-pages")]
    public async Task<ActionResult<ApiResponse<List<PageDto>>>> ReorderPages(ReorderPagesCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PageDto>>> UpdatePage(long id, [FromForm] UpdatePage req)
    {
        var command = new UpdatePageCommand
        {
            Id = id,
            PageNumber = req.PageNumber,
            ImageUrl = req.ImageUrl
        };

        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete]
    public async Task<ActionResult<ApiResponse<object>>> DeletePage(DeletePageCommand command)
    {
        var success = await _mediator.Send(command);
        return success ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }
}