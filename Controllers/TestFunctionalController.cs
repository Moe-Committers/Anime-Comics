using anime_comics.DB;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/testing")]
public class TestFunctionalController : ControllerBase
{
    private readonly database _db;
    public TestFunctionalController(database db, IImageService image)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult> testUpdate()
    {
        var activeUsers = await _db.users.CountAsync(u => u.status == Status.Active);
        return Ok(activeUsers);
    }
}