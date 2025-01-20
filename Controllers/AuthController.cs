using anime_comics.Features.Auth.Commands.Login;
using anime_comics.Features.Auth.Commands.Logout;
using anime_comics.Features.Auth.Commands.RefreshToken;
using anime_comics.Features.Auth.Commands.Register;
using anime_comics.Features.Auth.Queries.GetUser;
using anime_comics.Utils.DTOs.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public async Task<ActionResult<AuthResponse>> Register(RegisterCommand command)
    {
        var response = await _mediator.Send(command);
        
        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginCommand command)
    {
        var response = await _mediator.Send(command);
        
        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var query = new GetUserQuery(userId);
        var user = await _mediator.Send(query);
        return Ok(user);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Refresh token not found");
        }

        var command = new LogoutCommand(userId, refreshToken);
        var result = await _mediator.Send(command);
        
        if (result)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }
        
        return BadRequest();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Refresh token not found");
        }

        var command = new RefreshTokenCommand(refreshToken);
        var response = await _mediator.Send(command);

        SetRefreshTokenCookie(response.RefreshToken);
        
        return Ok(response);
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