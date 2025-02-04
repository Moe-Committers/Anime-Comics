using anime_comics.Utils.Enum;

namespace anime_comics.Utils.DTOs.Authentication;

public class UserDto{
    public long Id {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public int Age {get; set;}
    public Role role {get; set;}
    public Status status {get; set;}
    public DateTime CreatedAt {get; set;}
    public string Profile {get; set;}
}