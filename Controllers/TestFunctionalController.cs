using System.Security.Claims;
using anime_comics.DB;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.ResponseHelper;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/testing")]
public class TestFunctionalController : ControllerBase
{
    private readonly database _db;
    private readonly IImageService _image;
    public TestFunctionalController(database db, IImageService image)
    {
        _db = db;
        _image = image;
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse<UserDto>>> testUpdate([FromForm] UploadImageRequest request) /// u can use dto instead meaning the error will confirmg ImageUrl input to imageurl instead to lowercase();
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var user = await _db.users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundExceptions("User not founded!");

        if (!string.IsNullOrEmpty(user.Profile))
            _image.DeleteImage(user.Profile);

        user.Profile = await _image.UploadImage(request.ImageUrl, "Profile-pic");
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(ResHelper.Success(user.Adapt<UserDto>()));
    }
}

public class UploadImageRequest {
    public IFormFile ImageUrl {get; set;}
}