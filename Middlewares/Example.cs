namespace anime_comics.Middlewares;

public class Example{
    private readonly RequestDelegate _next;
    private readonly ILogger<Example> _logger;
    public Example(RequestDelegate next , ILogger<Example> logger){
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext http){
        if(http.Request.Path.StartsWithSegments("/api/example")){
            _logger.LogInformation("hellow world from middleware");
        }
        await _next(http);
    }
}