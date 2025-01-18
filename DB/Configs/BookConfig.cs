using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class BookConfig : IEntityTypeConfiguration<Books> {
    public void Configure(EntityTypeBuilder<Books> builder){

    }
}