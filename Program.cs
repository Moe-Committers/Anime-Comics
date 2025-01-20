

using anime_comics.Utils.Helpers.Extensions;
using anime_comics.Utils.Configs;
using anime_comics.Utils;
using Mapster;
using anime_comics.DB;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddMapster();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.useMiddleware();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();