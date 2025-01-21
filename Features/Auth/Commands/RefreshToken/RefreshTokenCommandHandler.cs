using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using anime_comics.DB;
using anime_comics.Models;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Helpers.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace anime_comics.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly database _db;
    private readonly IConfiguration _config;

    public RefreshTokenCommandHandler(database db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var token = await _db.refreshTokens
            .Include(u => u.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, ct);

        if (token == null)
        {
            throw new BadRequestExceptions("invalid token");
        }

        if (!token.IsActive)
        {
            throw new BadRequestExceptions("token is not active");
        }

        var accessToken = GenerateJwtToken(token.User);

        var newRefreshToken = new RefreshTokens
        {
            UserId = token.User.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpireAt = DateTime.UtcNow.AddDays(10),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = "0.0.0.0"
        };

        _db.refreshTokens.Remove(token);
        _db.refreshTokens.Add(newRefreshToken);
        await _db.SaveChangesAsync(ct);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = DateTime.UtcNow.AddDays(10),
            User = token.User.Adapt<UserDto>()
        };
    }

    private string GenerateJwtToken(Users user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role , user.role.ToString()),
            new Claim("status" , user.status.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            signingCredentials: cred,
            expires: DateTime.UtcNow.AddDays(7)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}