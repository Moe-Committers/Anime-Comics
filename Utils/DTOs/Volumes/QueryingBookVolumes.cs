namespace anime_comics.Utils.DTOs.Volumes;

public class QueryingBookVolumes : PaginateDto
{
    public string? Search { get; set; }
    public string? sort { get; set; } = "created";
    public bool IsAscending { get; set; } = false;
}