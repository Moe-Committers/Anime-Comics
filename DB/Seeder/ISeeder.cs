namespace anime_comics.DB.Seeder;

public interface ISeeder {
    Task Seed(database db);
    bool ShouldRun(database db);
}