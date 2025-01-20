using anime_comics.Features.Category.Commands.Create;
using anime_comics.Features.Category.Commands.Delete;
using anime_comics.Features.Category.Commands.Update;
using anime_comics.Features.Category.Queries.GetCategories;
using anime_comics.Features.Category.Queries.GetCategory;
using anime_comics.Utils.DTOs.Category;
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

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var query = new GetCategoriesQuery();
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDetailDto>> GetCategory(long id)
    {
        var query = new GetCategoryQuery(id);
        var category = await _mediator.Send(query);
        return Ok(category);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<long>> CreateCategory(CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(long id, [FromBody] string name)
    {
        var command = new UpdateCategoryCommand(id, name);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(long id)
    {
        var command = new DeleteCategoryCommand(id);
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }
}