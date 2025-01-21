using anime_comics.Utils.Enum;

namespace anime_comics.Utils.DTOs.Category;

public class CategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Status status {get; set;}
    public string Icon {get; set;}
    public int Order {get; set;}
    public int BookCount { get; set; }
}