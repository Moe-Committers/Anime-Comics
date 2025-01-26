namespace anime_comics.Utils.DTOs.Chapters;

public class ChapterDto
{
    public long Id { get; set; }
    public long VolumeId { get; set; }
    public int ChapNo { get; set; }
    public string Title { get; set; }
    public int PageCount { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Description { get; set; }
}