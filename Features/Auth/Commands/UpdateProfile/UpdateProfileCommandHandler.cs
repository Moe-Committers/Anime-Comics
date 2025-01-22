using anime_comics.DB;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Helpers.Exceptions;
using anime_comics.Utils.Helpers.Services.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserDto>
{
    private readonly database _db;
    private readonly IImageService _img;
    public UpdateProfileCommandHandler(database db, IImageService img)
    {
        _db = db;
        _img = img;
    }

    public async Task<UserDto> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await _db.users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            throw new NotFoundExceptions("User not founded!");
        }
        user.Name = request.Name ?? user.Name;
        if (request.Img != null)
        {
            if (!string.IsNullOrEmpty(user.Profile))
            {
                _img.DeleteImage(user.Profile);
            }
            user.Profile = await _img.UploadImage(request.Img, "Profile-pic");
        }
        user.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return user.Adapt<UserDto>();
    }
}