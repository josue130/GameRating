using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using GameRatingAPI;
using GameRatingAPI.Services;
using GameRatingAPI.Repository;
using GameRatingAPI.Utility;
using GameRatingAPI.Endpoints;
using GameRatingAPI.Repository.IRepository;
using GameRatingAPI.GraphQL;
using GameRatingAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddAuthorization()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

builder.Services.AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( s => {
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GameRatingAPI",
        Description = "With GameRatingAPI, you can rate games, add comments, and view detailed information about them.",
        Contact = new OpenApiContact
        {
            Name = "Josué López Mendoza",
            Url = new Uri("https://portfoliojlm.netlify.app/")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit/")
        }
    });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            }, new string[]{}
        }
    });
});
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;

    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, 
        IssuerSigningKeys = Keys.GetSecret(builder.Configuration),
        ClockSkew = TimeSpan.Zero 
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireClaim("admin"));
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    await TypedResults.BadRequest(
        new { type = "Error", message = "An unexpected error occurred.", status = 500 })
    .ExecuteAsync(context);
}));
app.UseStatusCodePages();

app.UseStaticFiles();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();

app.MapGet("/", () => "With GameRatingAPI, you can rate games, add comments, and view detailed information about them.");
app.MapGroup("/genres").MapGenres();
app.MapGroup("/games").MapGames();
app.MapGroup("/game/{gameId:int}/comments").MapComments();
app.MapGroup("/auth").MapUsers();


app.Run();
