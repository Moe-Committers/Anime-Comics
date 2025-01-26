using anime_comics.Features.Page.Commands.Add;
using anime_comics.Features.Page.Commands.Delete;
using anime_comics.Features.Page.Commands.ReorderPages;
using anime_comics.Features.Page.Commands.Update;
using anime_comics.Features.Page.Queries.GetChapterPages;
using anime_comics.Features.Page.Queries.GetPage;
using anime_comics.Utils.Attributes;
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

    [HttpGet("chapter/{chapterId}")]
    public async Task<ActionResult<ApiResponse<List<PageDto>>>> GetChapterPages(
        long chapterId,
        [FromQuery] QueryingChapterPages req)
    {
        var query = new GetChapterPagesQuery
        {
            ChapterId = chapterId,
            Page = req.Page,
            PageSize = req.PageSize
        };

        var pages = await _mediator.Send(query);
        return Ok(ResHelper.Success(pages));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PageDto>>> GetPage(long id)
    {
        var query = new GetPageQuery
        {
            PageId = id
        };

        var page = await _mediator.Send(query);
        return Ok(ResHelper.Success(page));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost("chapter/{chapterId}")]
    public async Task<ActionResult<ApiResponse<List<PageDto>>>> AddPages(
        long chapterId,
        [FromForm] List<IFormFile> images)
    {
        var command = new AddPagesCommand
        {
            ChapterId = chapterId,
            Images = images
        };

        var pages = await _mediator.Send(command);
        return Ok(ResHelper.Success(pages));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("chapter/{chapterId}/reorder")]
    public async Task<ActionResult<ApiResponse<List<PageDto>>>> ReorderPages(
        long chapterId,
        [FromBody] List<PageOrder> newOrder)
    {
        var command = new ReorderPagesCommand
        {
            ChapterId = chapterId,
            NewOrder = newOrder
        };

        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("chapter/{chapterId}/page/{id}")]
    public async Task<ActionResult<ApiResponse<PageDto>>> UpdatePage(
        long chapterId,
        long id,
        [FromForm] UpdatePageDto req)
    {
        var command = new UpdatePageCommand
        {
            ChapterId = chapterId,
            PageId = id,
            PageNumber = req.PageNumber,
            Image = req.Image
        };

        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete("chapter/{chapterId}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePages(
        long chapterId,
        [FromBody] List<long> pageIds)
    {
        var command = new DeletePagesCommand
        {
            ChapterId = chapterId,
            PageIds = pageIds
        };

        var success = await _mediator.Send(command);
        return success
            ? Ok(ResHelper.Success<object>())
            : NotFound(ResHelper.Error<object>());
    }
}