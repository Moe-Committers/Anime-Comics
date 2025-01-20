using anime_comics.Utils.DTOs.Books;

namespace anime_comics.Utils.DTOs.Category;

public class CategoryDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<BookDto> Books { get; set; }
}