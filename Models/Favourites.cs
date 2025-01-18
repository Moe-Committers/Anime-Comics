namespace anime_comics.Models;

public class Favourites : BaseEntity {
    public long UserId {get; set;}
    public long BookId {get; set;}
    public Users User {get; set;}
    public Books Book {get; set;}
}