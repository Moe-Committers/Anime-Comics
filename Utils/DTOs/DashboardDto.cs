namespace anime_comics.Utils.DTOs;

public class DashboardDto
{
    public int TotalBooks { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalCategories { get; set; }
    public List<MonthlyData> MonthlyNewUsers { get; set; } = new();
    public List<MonthlyData> MonthlyNewBooks { get; set; } = new();
}

public class MonthlyData
{
    public int Month { get; set; }
    public int Count { get; set; }
}