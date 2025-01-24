using anime_comics.Utils.Helpers.ResponseHelper;

public class ApiResponse<T> where T : class {
    public bool? Success {get; set;} = false;
    public string? Message {get; set;}
    public T? Data {get; set;} = null;
    public PaginateResponse? Paginate {get; set;}
    public Dictionary<string, string> Errors {get; set;}
}