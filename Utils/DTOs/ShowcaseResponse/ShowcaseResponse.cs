using anime_comics.Utils.DTOs.Books;

namespace anime_comics.Utils.DTOs.ShowcaseResponse;

public class ShowcaseResponse
{
    public IEnumerable<ShowcaseSection> Sections { get; set; } = new List<ShowcaseSection>();
}

public class ShowcaseSection
{
    public string Title { get; set; }
    public IEnumerable<PublishBookDto> Books { get; set; }
}