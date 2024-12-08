using AutoMapper;
using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;
using GameRaitingAPI.Filter;
using GameRaitingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace GameRaitingAPI.Endpoints
{
    public static class GameEndpoints
    {
        public static RouteGroupBuilder MapGames(this RouteGroupBuilder group)
        {
            group.MapPost("/", AddNewGame).DisableAntiforgery();
            group.MapGet("/", GetAllGames);
            group.MapGet("/{id:int}", GetGameById);
            group.MapDelete("/{id:int}", Delete);
            group.MapGet("/get_by_name/{name}", GetGameByName);
            group.MapPut("/{id:int}", Update).DisableAntiforgery();
            return group;
        }

        static async Task<Results<Created<GameDTO>, ValidationProblem>> AddNewGame([FromForm] CreateGameDTO createGameDTO,
            IGameRepository repository, IMapper mapper)
        { 
            Game game = mapper.Map<Game>(createGameDTO);

            int id = await repository.Add(game);

            GameDTO gameDTO = mapper.Map<GameDTO>(game);
            return TypedResults.Created($"/games/{id}", gameDTO);
        }

        static async Task<Results<Ok<GameDTO>, NotFound>> GetGameById(int id,
            IGameRepository repository, IMapper mapper)
        {
            Game? game = await repository.GetGameById(id);

            if (game is null)
            {
                return TypedResults.NotFound();
            }

            GameDTO gameDTO = mapper.Map<GameDTO>(game);
            return TypedResults.Ok(gameDTO);
        }

        static async Task<Ok<List<GameDTO>>> GetGameByName(string name,
            IGameRepository repository, IMapper mapper)
        {
            List<Game> game = await repository.GetGameByName(name);
            List<GameDTO> gameDto = mapper.Map<List<GameDTO>>(game);
            return TypedResults.Ok(gameDto);
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IGameRepository repository)
        {
            Game? gameDB = await repository.GetGameById(id);

            if (gameDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,[FromForm] CreateGameDTO createGameDTO,
            IGameRepository repository, IMapper mapper)
        {
            Game? gameDB = await repository.GetGameById(id);

            if (gameDB is null)
            {
                return TypedResults.NotFound();
            }

            Game game = mapper.Map<Game>(createGameDTO);
            game.Id = id;
            

            await repository.Update(game);
       
            return TypedResults.NoContent();
        }
        static async Task<Ok<List<GameDTO>>> GetAllGames(IGameRepository repository, IMapper mapper)
        {
            List<Game> game = await repository.GetAllGames();

            List<GameDTO> gameDto = mapper.Map<List<GameDTO>>(game);
            return TypedResults.Ok(gameDto);
        }

    }
}
