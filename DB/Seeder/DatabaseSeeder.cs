namespace anime_comics.DB.Seeder;

public class DatabaseSeeder{
    private static readonly ISeeder[] Seeders = new ISeeder[]{
        new AdminSeeder()
    };

    public static async Task SeedData(IServiceProvider service){
        using var scope = service.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<database>();

        foreach(var seeder in Seeders){
            if(seeder.ShouldRun(db)){
                await seeder.Seed(db);
            }
        }
    }
}