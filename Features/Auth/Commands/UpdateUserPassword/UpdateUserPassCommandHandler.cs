
using anime_comics.DB;
using anime_comics.Utils.Helpers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Commands.UpdateUserPassword;

public class UpdateUserPassCommandHandler : IRequestHandler<UpdateUserPassCommand , bool> {
    private readonly database _db;
    public UpdateUserPassCommandHandler(database db){
        _db = db;
    }

    public async Task<bool> Handle(UpdateUserPassCommand request , CancellationToken ct){
        var user = await _db.users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if(user == null){
            throw new NotFoundExceptions("User not founded!");
        }
        if(!BCrypt.Net.BCrypt.Verify(request.CurrentPassword , user.Password)){
            throw new InvalidCurrentPassword("Your old Password is not correct");
        }
        if(request.NewPassword != request.ConfirmPassword ){
            throw new InvalidConfirmPassword("Invalid Confirm password!");
        }       
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _db.SaveChangesAsync();
        return true;
    }
}