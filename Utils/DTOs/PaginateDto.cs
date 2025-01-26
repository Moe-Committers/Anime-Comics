namespace anime_comics.Utils.DTOs;

public abstract class PaginateDto{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}