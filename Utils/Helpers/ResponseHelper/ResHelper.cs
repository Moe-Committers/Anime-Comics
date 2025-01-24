namespace anime_comics.Utils.Helpers.ResponseHelper;

public static class ResHelper {
    public static ApiResponse<T> Success<T>(T? data = null , PaginateResponse? paginate = null , string message = "Success") where T : class{
    
        return new ApiResponse<T>{
            Success = true,
            Message = message,
            Data = data,
            Paginate = paginate
        };
    }
    public static ApiResponse<T> Error<T>(string message = "Errors!" , Dictionary<string , string> errors = null) where T : class{
        return new ApiResponse<T>{
            Success = false,
            Message = message,
            Errors = errors ?? new Dictionary<string, string>()
        };
    }
}