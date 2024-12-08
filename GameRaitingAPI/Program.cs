using FluentValidation;
using GameRaitingAPI;
using GameRaitingAPI.Endpoints;
using GameRaitingAPI.Repository;
using GameRaitingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddOutputCache();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");
app.MapGroup("/genres").MapGenres();


app.Run();
