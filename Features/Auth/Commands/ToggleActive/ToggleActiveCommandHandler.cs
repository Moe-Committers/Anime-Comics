using anime_comics.DB;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Commands.ToggleActive;

public class ToggleActiveCommandHandler : IRequestHandler<ToggleActiveCommand, UserDto>
{
    private readonly database _db;

    public ToggleActiveCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<UserDto> Handle(ToggleActiveCommand request, CancellationToken ct)
    {
        var user = await _db.users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            throw new NotFoundExceptions("the user is actually not founded");
        }
        user.status = user.status == Status.Active ? Status.InActive : Status.Active;
        await _db.SaveChangesAsync(ct);
        return user.Adapt<UserDto>();
    }
}