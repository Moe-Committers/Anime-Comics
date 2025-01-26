using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class PageConfig : IEntityTypeConfiguration<Pages>
{
    public void Configure(EntityTypeBuilder<Pages> builder)
    {
        builder.HasOne(p => p.Chapter)
               .WithMany(b => b.Pages)
               .HasForeignKey(p => p.ChapterId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.ChapterId });
    }
}