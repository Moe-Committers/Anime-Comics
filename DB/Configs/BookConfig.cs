using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class BookConfig : IEntityTypeConfiguration<Books>
{
    public void Configure(EntityTypeBuilder<Books> builder)
    {
        builder.HasOne(b => b.Users)
               .WithMany(u => u.Books)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Categories)
               .WithMany(c => c.Books);

        builder.HasMany(b => b.Pages)
               .WithOne(p => p.Book)
               .HasForeignKey(p => p.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Comments)
               .WithOne(c => c.Book)
               .HasForeignKey(c => c.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Favourites)
               .WithOne(f => f.Book)
               .HasForeignKey(f => f.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.Title).IsUnique();
        builder.HasIndex(b => b.Description);
    }
}