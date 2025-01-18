using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class FavouriteConfig : IEntityTypeConfiguration<Favourites> {
    public void Configure(EntityTypeBuilder<Favourites> builder){

    }
}