using anime_comics.Models;

namespace anime_comics.DB.Seeder;

public class CategorySeeder : ISeeder {
    private static readonly string[] DefaultCategories = new[]
    {
        "Action", "Adventure", "Comedy", "Drama", "Fantasy",
        "Horror", "Mystery", "Romance", "Sci-Fi", "Slice of Life"
    };

    public bool ShouldRun(database db){
        return !db.categories.Any();
    }

    public async Task Seed(database db){
        var categories = DefaultCategories.Select(name => new Categories {
            Name = name
        });

        db.categories.AddRange(categories);
        await db.SaveChangesAsync();
    }
}