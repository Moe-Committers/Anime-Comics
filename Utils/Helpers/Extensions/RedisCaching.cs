namespace anime_comics.Utils.Helpers.Extensions;

public static class RedisCaching{
    public static void UseRedisService(this IServiceCollection service , IConfiguration config){
        var redisConnectionString = config.GetValue<string>("Redis:Con");
        service.AddStackExchangeRedisCache(opt => {
            opt.Configuration = redisConnectionString;
            opt.InstanceName = "AnimeCache";
        });
    }
}