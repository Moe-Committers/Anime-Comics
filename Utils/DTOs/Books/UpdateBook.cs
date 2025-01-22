namespace anime_comics.Utils.DTOs.Books;

public class UpdateBook
{
    public string? Title { get; init; }
    public string? Description { get; set; }
    public string? Author { get; init; }
    public IFormFile? ImageUrl { get; init; }
    public ICollection<long>? CategoryIds { get; init; } = new List<long>();
}
