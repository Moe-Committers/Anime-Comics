using anime_comics.DB;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly database _db;

    public GetUserQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken ct)
    {
        var user = await _db.users.FirstOrDefaultAsync(u => u.id == request.UserId, ct);
        if (user == null)
        {
            throw new NotFoundExceptions("user not found");
        }
        return user.Adapt<UserDto>();
    }
}