using anime_comics.DB;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.Enum;
using anime_comics.Utils.Helpers.ResponseHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly database _db;

    public DashboardController(database db)
    {
        _db = db;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetDashboardStats()
    {
        var totalBooks = await _db.books.CountAsync();
        var totalUsers = await _db.users.CountAsync(u => u.role != Role.Admin);
        var activeUsers = await _db.users.CountAsync(u => u.status == Status.Active && u.role != Role.Admin);
        var totalCategories = await _db.categories.CountAsync();
        
        var monthlyNewUsers = await _db.users
            .Where(u => u.CreatedAt.Year == DateTime.UtcNow.Year && u.role != Role.Admin)
            .GroupBy(u => u.CreatedAt.Month)
            .Select(g => new MonthlyData
            {
                Month = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var monthlyNewBooks = await _db.books
            .Where(b => b.CreatedAt.Year == DateTime.UtcNow.Year)
            .GroupBy(b => b.CreatedAt.Month)
            .Select(g => new MonthlyData
            {
                Month = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        var stats = new DashboardDto
        {
            TotalBooks = totalBooks,
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            TotalCategories = totalCategories,
            MonthlyNewUsers = monthlyNewUsers,
            MonthlyNewBooks = monthlyNewBooks
        };

        return Ok(ResHelper.Success(stats));
    }
}