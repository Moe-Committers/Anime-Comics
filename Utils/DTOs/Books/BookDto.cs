namespace anime_comics.Utils.DTOs.Books;

public class BookDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string ImageUrl { get; set; }
    public string UserName { get; set; }
    public int Fav { get; set; }
    public List<Cate> Categories { get; set; }
}

public class Cate
{
    public long Id { get; set; }
    public string Name { get; set; }
}