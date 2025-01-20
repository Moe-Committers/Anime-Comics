namespace anime_comics.Utils.DTOs.Favourite;

public class FavouriteBookDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int FavCount { get; set; }
    public string CreatorName { get; set; }
    public DateTime AddedToFavouriteAt { get; set; }
}