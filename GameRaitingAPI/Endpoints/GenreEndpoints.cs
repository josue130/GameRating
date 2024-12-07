using GameRaitingAPI.Entitie;
using GameRaitingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameRaitingAPI.Endpoints
{
    public static class GenreEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllGenres);
            group.MapGet("/{id:int}", GetGenreById);
            group.MapPost("/", AddNewGenre);
            group.MapPut("/{id:int}", UpdateGenre);
            group.MapDelete("/{id:int}", DeleteGenre);

            return group;
        }

        static async Task<Ok<List<Genre>>> GetAllGenres(IGenreRepository repository)
        {
            List<Genre> genres = await repository.GetAll();
           
            return TypedResults.Ok(genres);
        }

        static async Task<Results<Ok<Genre>,NotFound>> GetGenreById(IGenreRepository repository, int id)
        {
            Genre? genre = await repository.GetById(id);

            if (genre is null)
            {
                return TypedResults.NotFound();
            }


            return TypedResults.Ok(genre);
        }

        static async Task<Results<Created<Genre>, ValidationProblem>> AddNewGenre(IGenreRepository repository, Genre genre)
        {

            var id = await repository.Add(genre);
            
            return TypedResults.Created($"/genre/{id}", genre);
        }


        static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateGenre(int id,Genre genre,
            IGenreRepository repository)
        {
            var exist = await repository.Exist(id);

            if (!exist)
            {
                return TypedResults.NotFound();
            }

            genre.Id = id;
            await repository.Update(genre);
        
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteGenre(int id, IGenreRepository repository)
        {
            var exist = await repository.Exist(id);

            if (!exist)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
           
            return TypedResults.NoContent();
        }

    }
}
