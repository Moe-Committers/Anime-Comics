using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand , bool> {
    private readonly database _db;

    public LogoutCommandHandler(database db ){
        _db = db;
    }

    public async Task<bool> Handle(LogoutCommand request , CancellationToken ct){
        var token = await _db.refreshTokens.FirstOrDefaultAsync(rt => rt.UserId == request.UserId && rt.Token == request.RefreshToken ,ct);
        if(token != null){
            _db.refreshTokens.Remove(token);
            await _db.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
}