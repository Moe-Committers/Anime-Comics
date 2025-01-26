using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class ChapterConfig : IEntityTypeConfiguration<Chapters>
{
    public void Configure(EntityTypeBuilder<Chapters> builder)
    {
        builder.HasOne(p => p.Volume)
               .WithMany(b => b.Chapters)
               .HasForeignKey(p => p.VolumeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.VolumeId });
    }
}