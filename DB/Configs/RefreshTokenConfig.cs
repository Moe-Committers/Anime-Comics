using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokens> {
    public void Configure(EntityTypeBuilder<RefreshTokens> builder){

    }
}