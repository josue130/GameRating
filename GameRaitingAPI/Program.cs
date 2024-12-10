using FluentValidation;
using GameRaitingAPI;
using GameRaitingAPI.Endpoints;
using GameRaitingAPI.Repository;
using GameRaitingAPI.Repository.IRepository;
using GameRaitingAPI.Services;
using GameRaitingAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();




builder.Services.AddOutputCache();
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseStaticFiles();
app.UseOutputCache();

app.MapGet("/", () => "Hello World!");
app.MapGroup("/genres").MapGenres();
app.MapGroup("/games").MapGames();
app.MapGroup("/auth").MapUsers();


app.Run();
