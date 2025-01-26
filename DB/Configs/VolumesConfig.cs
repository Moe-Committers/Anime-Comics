using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class VolumesConfig : IEntityTypeConfiguration<Volumes>
{
    public void Configure(EntityTypeBuilder<Volumes> builder)
    {
        builder.HasOne(p => p.Book)
               .WithMany(b => b.Volumes)
               .HasForeignKey(p => p.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.BookId });
    }
}