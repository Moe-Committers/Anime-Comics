using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class UserConfig : IEntityTypeConfiguration<Users> {
    public void Configure(EntityTypeBuilder<Users> builder){

    }
}