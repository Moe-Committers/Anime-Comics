using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class FavouriteConfig : IEntityTypeConfiguration<Favourites>
{
    public void Configure(EntityTypeBuilder<Favourites> builder)
    {
        builder.HasOne(f => f.User)
               .WithMany(u => u.Favourites)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Book)
               .WithMany(b => b.Favourites)
               .HasForeignKey(f => f.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(f => new { f.UserId, f.BookId }).IsUnique();
    }
}