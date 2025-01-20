namespace anime_comics.Utils.DTOs.Books;

public class PageDto
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public int PageNumber { get; set; }
    public string ImageUrl { get; set; }
}