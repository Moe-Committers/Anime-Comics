using anime_comics.Utils.DTOs;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Utils.Helpers.Extensions;

public static class PaginationExtEnsions{
    public static async Task<PageResponse<T>> CreatePaginatedResponse<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    ){
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PageResponse<T>{
          Data = items,
          TotalCount = totalCount,
          PageNumber = pageNumber,
          PageSize = pageSize,
          TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize)  
        };
    }
}