namespace anime_comics.Models;

public class Volumes : BaseEntity{
    public Volumes(){
        Chapters = new HashSet<Chapters>();
    }
    public long BookId {get; set;}
    public int VolumeNo {get; set;}
    public string Title {get; set;}
    public string CoverImg {get; set;}
    public DateTime? ReleaseDate {get; set;}
    public string Description {get; set;}
    public virtual Books Book {get; set;}
    public virtual ICollection<Chapters> Chapters {get; set;}
}