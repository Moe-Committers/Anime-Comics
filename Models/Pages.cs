namespace anime_comics.Models;

public class Pages : BaseEntity {
    public int PageNumber {get; set;}
    public long BookId {get; set;}
    public string ImageUrl {get; set;}
    public Books Book {get; set;}
}