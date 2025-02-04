
using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Commands.UpdateUsersPassword;

public class UpdateUsersPassCommandHandler : IRequestHandler<UpdateUsersPassCommand, bool>
{
    private readonly database _db;
    public UpdateUsersPassCommandHandler(database db)
    {
        _db = db;
    }

    public async Task<bool> Handle(UpdateUsersPassCommand request, CancellationToken ct)
    {
        var user = await _db.users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            throw new NotFoundExceptions("User not founded!");
        }
        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new InvalidConfirmPassword("Invalid Confirm password!");
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _db.SaveChangesAsync();
        return true;
    }
}