using anime_comics.Features.Chapter.Commands.AddChapter;
using anime_comics.Features.Chapter.Commands.DeleteChapter;
using anime_comics.Features.Chapter.Commands.UpdateChapter;
using anime_comics.Features.Chapter.Queries.GetChapter;
using anime_comics.Features.Chapter.Queries.GetVolumeChapters;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs.Chapters;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.ResponseHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anime_comics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChaptersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChaptersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("volume/{volumeId}")]
    public async Task<ActionResult<ApiResponse<List<ChapterDto>>>> GetVolumeChapters(
        long volumeId,
        [FromQuery] QueryingVolumeChapters req)
    {
        var query = new GetVolumeChaptersQuery
        {
            VolumeId = volumeId,
            Page = req.Page,
            PageSize = req.PageSize
        };

        var chapters = await _mediator.Send(query);
        return Ok(ResHelper.Success(chapters));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> GetChapter(
        long id)
    {
        var query = new GetChapterQuery
        {
            ChapterId = id
        };

        var chapter = await _mediator.Send(query);
        return Ok(ResHelper.Success(chapter));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost("volume/{volumeId}")]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> AddChapter(
        long volumeId,
        [FromBody] AddChapterCommand command)
    {
        var newCom = command with
        {
            VolumeId = volumeId
        };
        var chapter = await _mediator.Send(newCom);
        return Ok(ResHelper.Success(chapter));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("volume/{volumeId}/chapter/{id}")]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> UpdateChapter(
        long volumeId,
        long id,
        [FromBody] UpdateChapterDto req)
    {
        var command = new UpdateChapterCommand
        {
            VolumeId = volumeId,
            ChapterId = id,
            ChapNo = req.ChapNo,
            Title = req.Title,
            ReleaseDate = req.ReleaseDate,
            Description = req.Description
        };

        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete("volume/{volumeId}/chapter/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteChapter(
        long volumeId,
        long id)
    {
        var command = new DeleteChapterCommand
        {
            VolumeId = volumeId,
            ChapterId = id
        };

        var success = await _mediator.Send(command);
        return success
            ? Ok(ResHelper.Success<object>())
            : NotFound(ResHelper.Error<object>());
    }
}