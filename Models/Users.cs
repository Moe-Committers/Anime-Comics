namespace anime_comics.Models;

public class Users : BaseEntity {
    public Users(){
        Books = new HashSet<Books>();
        Comments = new HashSet<Comments>();
        Favourites = new HashSet<Favourites>();
        RefreshTokens = new HashSet<RefreshTokens>();
    }
    public string Name {get; set;}
    public int Age {get; set;}
    public string Profile {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public virtual ICollection<Books> Books {get; set;}
    public virtual ICollection<Comments> Comments {get; set;}
    public virtual ICollection<Favourites> Favourites {get; set;}
    public virtual ICollection<RefreshTokens> RefreshTokens {get; set;}
}