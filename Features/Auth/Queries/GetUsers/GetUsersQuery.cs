using anime_comics.Utils.DTOs;
using anime_comics.Utils.DTOs.Authentication;
using MediatR;

namespace anime_comics.Features.Auth.Queries.GetUsers;

public record GetUsersQuery : IRequest<PageResponse<UserDto>> {
    public string? Search {get; set;}
    public long? Id {get; set;}
    public DateTime? FromDate {get; set;}
    public DateTime? ToDate {get; set;}
    public string? sort {get; set;} = "created";
    public bool IsAscending {get; set;} = false;
    public int Page {get; init; } = 1;
    public int PageSize {get; init;} = 10;
}