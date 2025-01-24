using anime_comics.Utils.DTOs;

namespace anime_comics.Utils.Helpers.ResponseHelper;

public static class ResHelper {
    public static ApiResponse<T> Success<T>(T? data = null , string message = "Success") where T : class{
    
        return new ApiResponse<T>{
            Success = true,
            Message = message,
            Data = data,
        };
    }
    public static ApiResponse<T> Error<T>(string message , Dictionary<string , string> errors = null) where T : class{
        return new ApiResponse<T>{
            Success = false,
            Message = message,
            Errors = errors ?? new Dictionary<string, string>()
        };
    }
}