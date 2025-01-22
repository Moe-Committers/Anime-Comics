using MediatR;

namespace anime_comics.Features.Page.Commands.Delete;

public record DeletePageCommand : IRequest<bool>{
    public long BookId {get; set;}
    public List<long> PageIds {get; init;} = new();
};