using anime_comics.Models;
using anime_comics.Utils.Enum;

namespace anime_comics.DB.Seeder;

public class AdminSeeder : ISeeder {
    public bool ShouldRun(database db){
        return !db.users.Any(u => u.role == Role.Admin);
    }

    public async Task Seed(database db){
        var adminUser = new Users{
            Name = "Admin",
            Email = "admin@admin.com",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Age = 20,
            role = Role.Admin,
            status = Status.Active,
            CreatedAt = DateTime.UtcNow
        };

        db.users.Add(adminUser);
        await db.SaveChangesAsync();
    }
}