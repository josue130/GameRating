using GameRaitingAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));



var app = builder.Build();




app.MapGet("/", () => "Hello World!");

app.Run();
