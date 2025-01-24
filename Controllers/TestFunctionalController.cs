using anime_comics.DB;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task testUpdate()
    {

    }
}