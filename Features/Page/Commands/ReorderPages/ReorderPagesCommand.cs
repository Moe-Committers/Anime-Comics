using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Page.Commands.ReorderPages;

public record ReorderPagesCommand : IRequest<List<PageDto>>{
    public long BookId {get; set;}
    public List<PageOrder> NewOrder {get; init;} = new();
}

public class PageOrder {
    public long PageId {get; init;}
    public int NewPageNumber {get; init;}
}