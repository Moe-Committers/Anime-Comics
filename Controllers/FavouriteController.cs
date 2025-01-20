using anime_comics.Features.Favourite.Commands.Add;
using anime_comics.Features.Favourite.Commands.Remove;
using anime_comics.Features.Favourite.Queries.GetFavourite;
using anime_comics.Features.Favourite.Queries.GetFavourites;
using anime_comics.Utils.DTOs.Favourite;
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
    public async Task<ActionResult<List<FavouriteBookDto>>> GetUserFavourites()
    {
        var query = new GetUserFavouritesQuery();
        var favourites = await _mediator.Send(query);
        return Ok(favourites);
    }

    [HttpGet("check/{bookId}")]
    public async Task<ActionResult<bool>> CheckFavouriteStatus(long bookId)
    {
        var query = new CheckFavouriteStatusQuery(bookId);
        var isFavourited = await _mediator.Send(query);
        return Ok(isFavourited);
    }

    [HttpPost("{bookId}")]
    public async Task<ActionResult> AddFavourite(long bookId)
    {
        var command = new AddFavouriteCommand(bookId);
        var success = await _mediator.Send(command);
        return success ? Ok() : BadRequest("Already in favourites");
    }

    [HttpDelete("{bookId}")]
    public async Task<ActionResult> RemoveFavourite(long bookId)
    {
        var command = new RemoveFavouriteCommand(bookId);
        var success = await _mediator.Send(command);
        return success ? Ok() : NotFound();
    }
}