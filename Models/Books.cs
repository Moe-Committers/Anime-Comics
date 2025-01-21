namespace anime_comics.Models;

public class Books : BaseEntity
{
    public Books()
    {
        Pages = new HashSet<Pages>();
        Favourites = new HashSet<Favourites>();
        Comments = new HashSet<Comments>();
        Categories = new HashSet<Categories>();
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public DateTime? Published_at {get; set;}
    public int Fav { get; set; }
    public string ImageUrl { get; set; }
    public long UserId { get; set; }
    public virtual Users Users { get; set; }
    public virtual ICollection<Pages> Pages { get; set; }
    public virtual ICollection<Favourites> Favourites { get; set; }
    public virtual ICollection<Comments> Comments { get; set; }
    public virtual ICollection<Categories> Categories { get; set; } // Categories for this book
}