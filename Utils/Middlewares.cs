using anime_comics.Middlewares;

namespace anime_comics.Utils;

public static class Middlewares {
    public static void useMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<Example>();
    }
}