namespace anime_comics.Utils.DTOs.Volumes;

public class UpdateVolumeDto
{
    public int? VolumeNo { get; set; }
    public string? Title { get; set; }
    public IFormFile? CoverImg { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Description { get; set; }
}