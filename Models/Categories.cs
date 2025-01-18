namespace anime_comics.Models;

public class Categories
{
    public long Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Books> Books { get; set; }
}