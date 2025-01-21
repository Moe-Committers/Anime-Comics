using anime_comics.Utils.DTOs.Books;

namespace anime_comics.Utils.DTOs.Category;

public class CategoryDetailDto : CategoryDto
{
    public List<BookDto> Books { get; set; }
}