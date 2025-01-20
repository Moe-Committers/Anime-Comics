namespace anime_comics.Utils.DTOs.Authentication;

public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiresIn { get; set; }
    public required UserDto User { get; set; }
}