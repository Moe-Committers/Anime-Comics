namespace anime_comics.Utils.DTOs.Chapters;

public class UpdateChapterDto
{
    public int? ChapNo { get; set; }
    public string? Title { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Description { get; set; }
}