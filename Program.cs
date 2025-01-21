

using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Configs;
using anime_comics.Utils;
using Mapster;
using anime_comics.DB;
using Microsoft.EntityFrameworkCore;
using anime_comics.Utils.Enum;
using anime_comics.DB.Seeder;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddMapster();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddAuthorization(options => {
    options.AddPolicy($"Status:{Status.Active}" , policy => policy.RequireClaim("status" , Status.Active.ToString()));
});
builder.Services.AddAuth(builder.Configuration);
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<database>(option =>{
    option.UseNpgsql(connectionString);
});
builder.Services.AddControllers();
builder.Services.UseImageService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await DatabaseSeeder.SeedData(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.useMiddleware();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();