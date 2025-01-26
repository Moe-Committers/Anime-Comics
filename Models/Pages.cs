namespace anime_comics.Models;

public class Pages : BaseEntity {
    public int PageNumber {get; set;}
    public long ChapterId {get; set;}
    public string ImageUrl {get; set;}
    public Chapters Chapter {get; set;}
}