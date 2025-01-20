namespace anime_comics.Utils.DTOs.Authentication;

public class AuthResponse{
    public string AccessToken {get; set;}
    public string RefreshToken {get; set;}
    public DateTime ExpiresIn {get; set;}
    public UserDto User {get; set;}
}