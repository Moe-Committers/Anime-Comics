using anime_comics.Models;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.DB;

public class database : DbContext {
    public database(DbContextOptions<database> options) : base(options){

    }

    public DbSet<Users> users {get; set;}
    public DbSet<Comments> comments {get; set;}
    public DbSet<Books> books {get; set;}
    public DbSet<Favourites> favourites {get; set;}
    public DbSet<Pages> pages {get; set;}
    public DbSet<RefreshTokens> refreshTokens {get; set;}
    public DbSet<Categories> categories {get; set;}
    public DbSet<SiteSetting> sitesettings {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}