using anime_comics.Features.Category.Commands.Create;
using anime_comics.Features.Category.Commands.Delete;
using anime_comics.Features.Category.Commands.Update;
using anime_comics.Features.Category.Queries.GetCategories;
using anime_comics.Features.Category.Queries.GetCategory;
using anime_comics.Utils.Attributes;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Category;
using anime_comics.Utils.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-active")]
    public async Task<ActionResult<PageResponse<CategoryDto>>> GetActiveCategories([FromQuery] GetCategoriesQuery query )
    {
        var newQuery = query with {Toggle = true};
        var categories = await _mediator.Send(newQuery);
        return Ok(categories);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpGet]
    public async Task<ActionResult<PageResponse<CategoryDto>>> GetCategories([FromQuery] GetCategoriesQuery query )
    {
        var newQuery = query with {Toggle = false};
        var categories = await _mediator.Send(newQuery);
        return Ok(categories);
    }

    [HttpGet("get-active/{id}")]
    public async Task<ActionResult<CategoryDetailDto>> GetActiveCategory(long id)
    {
        var query = new GetCategoryQuery();
        var newQuery = query with {Id = id , Toggle = true};
        var category = await _mediator.Send(newQuery);
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDetailDto>> GetCategory(long id)
    {
        var query = new GetCategoryQuery();
        var newQuery = query with {Id = id , Toggle = false};
        var category = await _mediator.Send(newQuery);
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPost]
    public async Task<ActionResult<long>> CreateCategory([FromForm] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(long id, [FromForm] UpdateCategory req)
    {
        var command = new UpdateCategoryCommand {
            Id = id,
            Name = req.Name,
            status = req.status,
            Icon = req.Icon,
            Order = req.Order
        };
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [AuthorizeStatus(Status.Active)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(long id)
    {
        var command = new DeleteCategoryCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}