using System.Text.Json.Serialization;
using anime_comics.Utils.DTOs.Books;
using MediatR;

namespace anime_comics.Features.Book.Queries.GetBook;

public record GetBookQuery : IRequest<BookDetailDto>{
    public long Id {get; set;}
    [JsonIgnore]
    public bool published {get; set;} 
};