using anime_comics.Utils.Enum;

namespace anime_comics.Models;

public class Categories : BaseEntity
{
    public string Name { get; set; }
    public Status status {get; set;}
    public string Icon {get; set;}
    public int Order {get; set;}
    public virtual ICollection<Books> Books { get; set; }
}