using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Features.Auth.Commands.ToggleActive;
using anime_comics.Utils.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using anime_comics.Utils.Helpers.ResponseHelper;
using anime_comics.Features.Auth.Commands.UpdateProfile;
using anime_comics.Features.Auth.Commands.UpdateUsersPassword;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("toggle/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> ToggleUserStatus(long id, [FromBody] ToggleActive request)
    {
        var newcom = new ToggleActiveCommand
        {
            Id = id,
            Toggle = request.Toggle
        };
        var response = await _mediator.Send(newcom);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("change-avatar/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> ChangeAvatar(long id, [FromForm] UpdateUserProfile req)
    {
        var command = new UpdateProfileCommand
        {
            Id = id,
            Name = req.Name,
            Img = req.FileImg
        };
        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("change-password/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> ChangePassword(long id, [FromBody] UpdateUsersPassCommand command)
    {
        var newQuery = command with { Id = id };
        await _mediator.Send(newQuery);
        return Ok(ResHelper.Success<ActionResult>());
    }
}