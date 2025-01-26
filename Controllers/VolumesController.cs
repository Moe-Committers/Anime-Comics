using anime_comics.Features.Volume.Commands.AddVolume;
using anime_comics.Features.Volume.Commands.DeleteVolume;
using anime_comics.Features.Volume.Commands.UpdateVolume;
using anime_comics.Features.Volume.Queries.GetBookVolumes;
using anime_comics.Features.Volume.Queries.GetVolume;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs.Volumes;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.ResponseHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VolumesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VolumesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("book/{bookId}")]
    public async Task<ActionResult<ApiResponse<List<VolumeDto>>>> GetBookVolumes(
        long bookId,
        [FromQuery] QueryingBookVolumes req)
    {
        var query = new GetBookVolumesQuery
        {
            BookId = bookId,
            Page = req.Page,
            PageSize = req.PageSize
        };

        var volumes = await _mediator.Send(query);
        return Ok(ResHelper.Success(volumes));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VolumeDto>>> GetVolume(long id)
    {
        var query = new GetVolumeQuery
        {
            VolumeId = id
        };

        var volume = await _mediator.Send(query);
        return Ok(ResHelper.Success(volume));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost("book/{bookId}")]
    public async Task<ActionResult<ApiResponse<VolumeDto>>> AddVolume(
        long bookId,
        [FromForm] AddVolumeCommand command)
    {
        var newCom = command with {
            BookId = bookId
        };
        var volume = await _mediator.Send(newCom);
        return Ok(ResHelper.Success(volume));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("book/{bookId}/volume/{id}")]
    public async Task<ActionResult<ApiResponse<VolumeDto>>> UpdateVolume(
        long bookId,
        long id,
        [FromForm] UpdateVolumeDto req)
    {
        var command = new UpdateVolumeCommand
        {
            BookId = bookId,
            VolumeId = id,
            VolumeNo = req.VolumeNo,
            Title = req.Title,
            CoverImg = req.CoverImg,
            ReleaseDate = req.ReleaseDate,
            Description = req.Description
        };

        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete("book/{bookId}/volume/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteVolume(
        long bookId,
        long id)
    {
        var command = new DeleteVolumeCommand
        {
            BookId = bookId,
            VolumeId = id
        };

        var success = await _mediator.Send(command);
        return success
            ? Ok(ResHelper.Success<object>())
            : NotFound(ResHelper.Error<object>());
    }
}