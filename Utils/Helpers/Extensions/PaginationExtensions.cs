using anime_comics.Utils.Helpers.ResponseHelper;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Utils.Helpers.Extensions;

public static class PaginationExtEnsions
{
    public static async Task<ApiResponse<List<T>>> CreatePaginatedResponse<T , TSource>(
        this IQueryable<TSource> query,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return ResHelper.Success(items.Adapt<List<T>>(), new PaginateResponse
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }
}