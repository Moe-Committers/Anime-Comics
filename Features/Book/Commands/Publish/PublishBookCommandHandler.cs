using anime_comics.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Commands.Publish;

public class PublishBookCommandHandler : IRequestHandler<PublishBookCommand , bool> {
    private readonly database _db;

    public PublishBookCommandHandler(database db){
        _db = db;
    }

    public async Task<bool> Handle(PublishBookCommand request , CancellationToken ct){
        var books = await _db.books.FirstOrDefaultAsync(b => b.Id == request.Id);

        if(books == null) return false;

        if(request.publish){
            books.Published_at = DateTime.UtcNow;
        }else {
            books.Published_at = null;
        }

        await _db.SaveChangesAsync(ct);
        return true;
    }
}