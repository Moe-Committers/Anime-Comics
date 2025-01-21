namespace anime_comics.Utils.DTOs.Books;

public class BookDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime? Published_at {get; set;}
    public string ImageUrl { get; set; }
    public string UserName { get; set; }
    public int Fav { get; set; }
}