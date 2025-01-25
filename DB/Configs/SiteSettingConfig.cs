using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class SiteSettingConfig : IEntityTypeConfiguration<SiteSetting> {
    public void Configure(EntityTypeBuilder<SiteSetting> builder){
        builder.HasIndex(s => s.Title).IsUnique();
    }
}