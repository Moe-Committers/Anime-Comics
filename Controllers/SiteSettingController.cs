using anime_comics.Features.SiteSetting.Commands.Add;
using anime_comics.Features.SiteSetting.Commands.Delete;
using anime_comics.Features.SiteSetting.Commands.Update;
using anime_comics.Features.SiteSetting.Queries.GetSiteSetting;
using anime_comics.Features.SiteSetting.Queries.GetSiteSettings;
using anime_comics.Models;
using anime_comics.Utils.Helpers.ResponseHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SiteSettingController : ControllerBase {
    private readonly IMediator _mediatr;
    public SiteSettingController(IMediator mediatr){
        _mediatr = mediatr;
    }
    [HttpPost]
    public async Task<ActionResult<SiteSetting>> AddSite(AddSiteSetting command){
        var response = await _mediatr.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SiteSetting>> UpdateSite(long id , [FromForm] updateSite request){
        var command = new UpdateSiteSetting {
            Id = id,
            Title = request.Title,
            Logo = request.Logo
        };
        var response = await _mediatr.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteSite(long id){
        var command = new DeleteSiteSetting{
            Id = id
        };
        var response = await _mediatr.Send(command);
        return response ? Ok(ResHelper.Success<ActionResult>()) : NotFound(ResHelper.Error<ActionResult>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SiteSetting>> GetSite(GetSiteSetting command){
        var response = await _mediatr.Send(command);
        return Ok(ResHelper.Success(response));
    }

    [HttpGet]
    public async Task<ActionResult<List<SiteSetting>>> GetSites(){
        var command = new GetSiteSettings();
        var response = await _mediatr.Send(command);
        return Ok(ResHelper.Success(response));
    }
}

public class updateSite {
    public string Title {get; set;}
    public IFormFile Logo {get; set;}
}