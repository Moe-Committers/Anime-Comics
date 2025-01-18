using anime_comics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace anime_comics.DB.Configs;

public class CommentConfig : IEntityTypeConfiguration<Comments> {
    public void Configure(EntityTypeBuilder<Comments> builder){

    }
}