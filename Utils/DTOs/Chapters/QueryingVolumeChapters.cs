namespace anime_comics.Utils.DTOs.Chapters;

public class QueryingVolumeChapters : PaginateDto
{
    public string? Search {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
}
