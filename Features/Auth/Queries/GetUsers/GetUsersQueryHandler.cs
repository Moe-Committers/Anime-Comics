using anime_comics.DB;
using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Authentication;
using anime_comics.Utils.Enum;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Auth.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery , PageResponse<UserDto>>{
    private readonly database _db;

    public GetUsersQueryHandler(database db){
        _db = db;
    }

    public async Task<PageResponse<UserDto>> Handle(GetUsersQuery request , CancellationToken ct){
        var query = _db.users.AsQueryable();

        if(!string.IsNullOrEmpty(request.Search)){
            query = query.Where(u => u.Name.ToLower().Contains(request.Search) 
            || u.Age == int.Parse(request.Search)
            || u.Email.ToLower().Contains(request.Search));
        }

        if(request.Id.HasValue){
            query = query.Where(u => u.Id == request.Id);
        }

        if(request.FromDate.HasValue){
            query = query.Where(u => u.CreatedAt >= request.FromDate);
        }

        if(request.ToDate.HasValue){
            query = query.Where(u => u.CreatedAt <= request.ToDate);
        }

        query = request.sort?.ToLower() switch {
            "name" => request.IsAscending
                ? query.OrderBy(u => u.Name)
                : query.OrderByDescending(b => b.Name),
            "age" => request.IsAscending
                ? query.OrderBy(u => u.Age)
                : query.OrderByDescending(u => u.Age),
            "created" => request.IsAscending
                ? query.OrderBy(u => u.CreatedAt)
                : query.OrderByDescending(u => u.CreatedAt),
            _ => query.OrderByDescending(u => u.CreatedAt)
        };

        var totalCount = await _db.users.CountAsync();

        var users = await query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .Where(u => u.role != Role.Admin)
        .Select( u => new UserDto {
            Name = u.Name,
            Email = u.Email,
            Age = u.Age,
            Profile = u.Profile
        })
        .ToListAsync();

        var data = users.Adapt<List<UserDto>>();

        return new PageResponse<UserDto> {
            Data = data,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize,
            TotalPage = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}