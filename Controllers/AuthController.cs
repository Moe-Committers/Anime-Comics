using anime_comics.Features.Auth.Commands.Login;
using anime_comics.Features.Auth.Commands.Logout;
using anime_comics.Features.Auth.Commands.RefreshToken;
using anime_comics.Features.Auth.Commands.Register;
using anime_comics.Features.Auth.Queries.GetUser;
using anime_comics.Features.Auth.Queries.GetUsers;
using anime_comics.Features.Auth.Commands.UpdateProfile;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using anime_comics.Features.Auth.Commands.UpdateUserPassword;
using anime_comics.Utils.Helpers.ResponseHelper;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register(RegisterCommand command)
    {
        var response = await _mediator.Send(command);
        
        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(ResHelper.Success(response));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(LoginCommand command)
    {
        var response = await _mediator.Send(command);
        
        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(ResHelper.Success(response));
    }

    [AuthorizeStatus(Status.Active)]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUser()
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var query = new GetUserQuery(userId);
        var user = await _mediator.Send(query);
        return Ok(ResHelper.Success(ResHelper.Success(user)));
    }

    [AuthorizeStatus(Status.Active)]
    [HttpPost("logout")]
    public async Task<ApiResponse<ActionResult>> Logout()
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return ResHelper.Error<ActionResult>("Refresh token not found");
        }

        var command = new LogoutCommand(userId, refreshToken);
        var result = await _mediator.Send(command);
        
        if (result)
        {
            Response.Cookies.Delete("refreshToken");
            return ResHelper.Success<ActionResult>(null, null, "Success");
        }
        
        return ResHelper.Error<ActionResult>("Errors!");
    }

    [Authorize]
    [HttpPut("change-avatar")]
    public async Task<ActionResult<ApiResponse<UserDto>>> ChangeAvatar([FromForm] UpdateUserProfile req){
        var command = new UpdateProfileCommand();
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var newQuery = command with {Id = userId , Name = req.Name , Img = req.FileImg};
        var response = await _mediator.Send(newQuery);
        return Ok(ResHelper.Success(response));
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<ActionResult<ApiResponse<object>>> ChangePassword(UpdateUserPassCommand command){
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var newQuery = command with {Id = userId};
        await _mediator.Send(newQuery);
        return Ok(ResHelper.Success<ActionResult>());
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetUsers([FromQuery] GetUsersQuery query){
        var response = await _mediator.Send(query);
        return Ok(ResHelper.Success(response.Data , response.Paginate));

    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return NotFound(ResHelper.Error<ActionResult>("Refresh token not found"));
        }

        var command = new RefreshTokenCommand(refreshToken);
        var response = await _mediator.Send(command);

        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(ResHelper.Success(response));
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}