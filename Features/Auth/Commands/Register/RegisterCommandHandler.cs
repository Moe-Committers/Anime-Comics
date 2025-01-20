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

namespace anime_comics.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly database _db;
    private readonly IConfiguration _config;

    public RegisterCommandHandler(database db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _db.users.AnyAsync(u => u.Email == request.Email))
        {
            throw new BadRequestExceptions("email already exist");
        }

        var user = new Users
        {
            Name = request.Name,
            Age = request.Age,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _db.users.Add(user);
        await _db.SaveChangesAsync(ct);

        return await GenerateAuthResponse(user);
    }

    private async Task<AuthResponse> GenerateAuthResponse(Users user)
    {
        var accessToken = GenerateJwtToken(user);

        var refreshToken = new RefreshTokens
        {
            UserId = user.id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpireAt = DateTime.UtcNow.AddDays(10),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = "0.0.0.0"
        };

        _db.refreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = DateTime.UtcNow.AddDays(10),
            User = user.Adapt<UserDto>()
        };
    }

    private string GenerateJwtToken(Users user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
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