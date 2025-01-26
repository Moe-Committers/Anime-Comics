using anime_comics.Utils.Enum;

namespace anime_comics.Utils.DTOs.Authentication;

public class UserDto{
    public string Name {get; set;}
    public string Email {get; set;}
    public int Age {get; set;}
    public Role role {get; set;}
    public string Profile {get; set;}
}