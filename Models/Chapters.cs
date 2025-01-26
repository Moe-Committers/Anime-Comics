namespace anime_comics.Models;

public class Chapters : BaseEntity{
    public Chapters(){
        Pages = new HashSet<Pages>();
    }
    public long VolumeId {get; set;}
    public int ChapNo {get; set;}
    public string Title {get; set;}
    public int PageCount {get; set;}
    public DateTime? ReleaseDate {get; set;}
    public string Description {get; set;}
    public virtual Volumes Volume {get; set;}
    public virtual ICollection<Pages> Pages {get; set;}
}