using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Features.Auth.Commands.ToggleActive;
using anime_comics.Utils.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using anime_comics.Utils.Helpers.ResponseHelper;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase {
    private readonly IMediator _mediator;
    public UserManagementController(IMediator mediator){
        _mediator = mediator;
    }
    
    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("toggle/{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> ToggleUserStatus(ToggleActiveCommand command){
        var response = await _mediator.Send(command);
        return Ok(ResHelper.Success(response));
    }
}