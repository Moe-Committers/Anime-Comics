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

    [HttpPut]
    public async Task<ActionResult<ApiResponse<UserDto>>> testUpdate([FromForm] UploadImageRequest request) /// u can use dto instead meaning the error will confirmg ImageUrl input to imageurl instead to lowercase();
    {
        var data = new List<string>();
        foreach(var reqImg in request.ImageUrl){
            var res = await _image.UploadImage(reqImg, "Profile-pic");
            data.Add(res);
        }
        
        return Ok(ResHelper.Success(data));
    }
}

public class UploadImageRequest {
    public List<IFormFile> ImageUrl {get; set;}
}