using anime_comics.Features.Favourite.Commands.Add;
using anime_comics.Features.Favourite.Commands.Remove;
using anime_comics.Features.Favourite.Queries.GetFavourite;
using anime_comics.Features.Favourite.Queries.GetFavourites;
using anime_comics.Utils.DTOs.Favourite;
using anime_comics.Utils.Helpers.ResponseHelper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavouritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavouritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<FavouriteBookDto>>>> GetUserFavourites()
    {
        var query = new GetUserFavouritesQuery();
        var favourites = await _mediator.Send(query);
        return Ok(favourites);
    }

    [HttpGet("check/{bookId}")]
    public async Task<ActionResult<ApiResponse<object>>> CheckFavouriteStatus(long bookId)
    {
        var query = new CheckFavouriteStatusQuery(bookId);
        var isFavourited = await _mediator.Send(query);
        return isFavourited ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }

    [HttpPost("{bookId}")]
    public async Task<ActionResult<ApiResponse<object>>> AddFavourite(long bookId)
    {
        var command = new AddFavouriteCommand(bookId);
        var success = await _mediator.Send(command);
        return success ? Ok(ResHelper.Success<ActionResult>()) : BadRequest(ResHelper.Error<ActionResult>());
    }

    [HttpDelete("{bookId}")]
    public async Task<ActionResult<ApiResponse<object>>> RemoveFavourite(long bookId)
    {
        var command = new RemoveFavouriteCommand(bookId);
        var success = await _mediator.Send(command);
        return success ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }
}