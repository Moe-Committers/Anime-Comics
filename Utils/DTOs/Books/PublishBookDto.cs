namespace anime_comics.Utils.DTOs.Books;

public class PublishBookDto {
    public long Id {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Author {get; set;}
    public string ImageUrl {get; set;}
    public string UserName {get; set;}
    public int Fav {get; init;}
}