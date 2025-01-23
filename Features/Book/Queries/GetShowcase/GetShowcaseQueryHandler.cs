using anime_comics.DB;
using anime_comics.Utils.DTOs.Books;
using anime_comics.Utils.DTOs.ShowcaseResponse;
using anime_comics.Utils.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.Features.Book.Queries.GetShowcase;

public class GetShowcaseQueryHandler : IRequestHandler<GetShowcaseQuery, ShowcaseResponse>
{
    private readonly database _db;
    private const int ITEMS_PER_SECTION = 10;

    public GetShowcaseQueryHandler(database db)
    {
        _db = db;
    }

    public async Task<ShowcaseResponse> Handle(GetShowcaseQuery request, CancellationToken ct)
    {
        var sections = new List<ShowcaseSection>();

        var popularBooks = await _db.books
            .Include(b => b.Users)
            .Where(b => b.Published_at != null)
            .OrderByDescending(b => b.Fav)
            .Take(ITEMS_PER_SECTION)
            .Select(b => new PublishBookDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Author = b.Author,
                ImageUrl = b.ImageUrl,
                UserName = b.Users.Name,
                Fav = b.Fav
            })
            .ToListAsync(ct);

        sections.Add(new ShowcaseSection 
        { 
            Title = "Popular Books",
            Books = popularBooks
        });

        var featuredCategories = await _db.categories
            .Where(c => c.status == Status.Active)
            .Take(3) 
            .ToListAsync(ct);

        foreach (var category in featuredCategories)
        {
            var categoryBooks = await _db.books
                .Include(b => b.Users)
                .Where(b => 
                    b.Published_at != null && 
                    b.Categories.Any(c => c.Id == category.Id))
                .OrderByDescending(b => b.CreatedAt) 
                .Take(ITEMS_PER_SECTION)
                .Select(b => new PublishBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    UserName = b.Users.Name,
                    Fav = b.Fav,
                })
                .ToListAsync(ct);

            sections.Add(new ShowcaseSection
            {
                Title = $"Top {category.Name} Series",
                Books = categoryBooks
            });
        }

        return new ShowcaseResponse { Sections = sections };
    }
}