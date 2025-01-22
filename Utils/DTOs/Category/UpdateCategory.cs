using anime_comics.Utils.Enum;

namespace anime_comics.Utils.DTOs.Category;

public class UpdateCategory
{
    public string? Name {get; set;}
    public Status? status {get; set;}
    public IFormFile? Icon {get; set;}
    public int? Order {get; set;}
}