using AutoMapper;
using GameRatingAPI.DTOs;
using GameRatingAPI.Entitie;
using GameRatingAPI.Filter;
using GameRatingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace GameRatingAPI.Endpoints
{
    public static class GenreEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllGenres).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genre-get"));
            group.MapGet("/{id:int}", GetGenreById);

            group.MapPost("/", AddNewGenre)
                .AddEndpointFilter<ValidationFilter<CreateGenreDTO>>()
                .RequireAuthorization("admin");

            group.MapPut("/{id:int}", UpdateGenre)
                .AddEndpointFilter<ValidationFilter<CreateGenreDTO>>()
                .RequireAuthorization("admin");

            group.MapDelete("/{id:int}", DeleteGenre).RequireAuthorization("admin");

            return group;
        }

        static async Task<Ok<List<GenreDTO>>> GetAllGenres(IGenreRepository repository, IMapper mapper)
        {
            List<Genre> genres = await repository.GetAll();

            List<GenreDTO> genresDto = mapper.Map<List<GenreDTO>>(genres);

            return TypedResults.Ok(genresDto);
        }

        static async Task<Results<Ok<GenreDTO>, NotFound>> GetGenreById(IGenreRepository repository, IMapper mapper, int id)
        {
            Genre? genre = await repository.GetById(id);

            if (genre is null)
            {
                return TypedResults.NotFound();
            }

            GenreDTO genreDTO = mapper.Map<GenreDTO>(genre);


            return TypedResults.Ok(genreDTO);
        }

        static async Task<Results<Created<GenreDTO>, ValidationProblem>> AddNewGenre(IGenreRepository repository
            , CreateGenreDTO createGenreDTO, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            Genre genre = mapper.Map<Genre>(createGenreDTO);
            var id = await repository.Add(genre);
            await outputCacheStore.EvictByTagAsync("genre-get", default);
            GenreDTO genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Created($"/genre/{id}", genreDTO);
        }


        static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateGenre(int id, CreateGenreDTO createGenreDTO,
            IGenreRepository repository, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            var exist = await repository.Exist(id);

            if (!exist)
            {
                return TypedResults.NotFound();
            }

            Genre genre = mapper.Map<Genre>(createGenreDTO);
            genre.Id = id;
            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genre-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteGenre(int id, IGenreRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var exist = await repository.Exist(id);

            if (!exist)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genre-get", default);
            return TypedResults.NoContent();
        }

    }
}
