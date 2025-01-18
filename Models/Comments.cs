namespace anime_comics.Models;

public class Comments : BaseEntity {
    public long BookId {get; set;}
    public long UserId {get; set;}
    public long? parentId {get; set;}
    public string Content {get; set;}
    public virtual Books Book {get; set;}
    public virtual Users User {get; set;}
}