using anime_comics.Models;
using Microsoft.EntityFrameworkCore;

namespace anime_comics.DB;

public class database : DbContext {
    public database(DbContextOptions<database> options) : base(options){

    }

    public DbSet<Users> users;
    public DbSet<Comments> comments;
    public DbSet<Books> books;
    public DbSet<Favourites> favourites;
    public DbSet<Pages> pages;
    public DbSet<RefreshTokens> refreshTokens;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}