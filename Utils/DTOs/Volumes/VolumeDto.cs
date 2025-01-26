namespace anime_comics.Utils.DTOs.Volumes;

public class VolumeDto
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public int VolumeNo { get; set; }
    public string Title { get; set; }
    public string CoverImg { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Description { get; set; }
}