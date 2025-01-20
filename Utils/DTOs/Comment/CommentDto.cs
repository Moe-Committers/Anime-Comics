namespace anime_comics.Utils.DTOs.Comment;

public class CommentDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
}