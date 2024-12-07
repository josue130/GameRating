using GameRaitingAPI;
using GameRaitingAPI.Endpoints;
using GameRaitingAPI.Repository;
using GameRaitingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));


builder.Services.AddScoped<IGenreRepository, GenreRepository>();


var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.MapGroup("/genres").MapGenres();


app.Run();
