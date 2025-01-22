namespace anime_comics.Utils.Helpers.ResponseHelper;

public class ApiResponse<T> where T : class {
    public bool Success {get; set;}
    public string Message {get; set;}
    public T? Data {get; set;} = null;
    public Dictionary<string, string> Errors {get; set;}
}