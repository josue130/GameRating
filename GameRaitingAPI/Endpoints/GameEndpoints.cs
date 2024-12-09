using AutoMapper;
using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;
using GameRaitingAPI.Filter;
using GameRaitingAPI.Repository.IRepository;
using GameRaitingAPI.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace GameRaitingAPI.Endpoints
{
    public static class GameEndpoints
    {
        private static readonly string container = "games";
        public static RouteGroupBuilder MapGames(this RouteGroupBuilder group)
        {
            group.MapPost("/", AddNewGame).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateGameDTO>>();
            group.MapGet("/", GetAllGames).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("games-get")) ;
            group.MapGet("/{id:int}", GetGameById);
            group.MapDelete("/{id:int}", Delete);
            group.MapGet("/get_by_name/{name}", GetGameByName);
            group.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateGameDTO>>();
            return group;
        }

        static async Task<Results<Created<GameDTO>, ValidationProblem>> AddNewGame([FromForm] CreateGameDTO createGameDTO,
            IGameRepository repository, IMapper mapper, IOutputCacheStore outputCacheStore, IImageStorage imageStorage)
        { 
            Game game = mapper.Map<Game>(createGameDTO);

            if (createGameDTO.Photo is not null)
            {
                string url = await imageStorage.Store(container,createGameDTO.Photo);
                game.Photo = url;
            }

            int id = await repository.Add(game);
            await outputCacheStore.EvictByTagAsync("games-get", default);
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

        static async Task<Results<NoContent, NotFound>> Delete(int id, IGameRepository repository, 
            IOutputCacheStore outputCacheStore, IImageStorage imageStorage)
        {
            Game? gameDB = await repository.GetGameById(id);

            if (gameDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await imageStorage.Delete(gameDB.Photo, container);
            await outputCacheStore.EvictByTagAsync("games-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,[FromForm] CreateGameDTO createGameDTO,
            IGameRepository repository, IMapper mapper, IOutputCacheStore outputCacheStore, IImageStorage imageStorage)
        {
            Game? gameDB = await repository.GetGameById(id);

            if (gameDB is null)
            {
                return TypedResults.NotFound();
            }
            
            Game game = mapper.Map<Game>(createGameDTO);
            game.Id = id;
            game.Photo = gameDB.Photo;

            if (createGameDTO.Photo is not null)
            {
                string url = await imageStorage.Update(game.Photo,container, createGameDTO.Photo);
                game.Photo = url;
            }


            await repository.Update(game);
            await outputCacheStore.EvictByTagAsync("games-get", default);

            return TypedResults.NoContent();
        }
        static async Task<Ok<List<GameDTO>>> GetAllGames(IGameRepository repository, IMapper mapper, [FromQuery]int page = 1, [FromQuery] int recordsPerPage=4)
        {
            var pagination = new PaginationDTO { Page = page, GetRecordsPerPage = recordsPerPage };
            List<Game> game = await repository.GetAllGames(pagination);

            List<GameDTO> gameDto = mapper.Map<List<GameDTO>>(game);
            return TypedResults.Ok(gameDto);
        }

    }
}
