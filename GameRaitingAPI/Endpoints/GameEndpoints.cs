using AutoMapper;
using GameRatingAPI.DTOs;
using GameRatingAPI.Entitie;
using GameRatingAPI.Filter;
using GameRatingAPI.Repository.IRepository;
using GameRatingAPI.Services.IServices;
using GameRatingAPI.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace GameRatingAPI.Endpoints
{
    public static class GameEndpoints
    {
        private static readonly string container = "games";
        public static RouteGroupBuilder MapGames(this RouteGroupBuilder group)
        {
            group.MapPost("/", AddNewGame)
                .DisableAntiforgery()
                .AddEndpointFilter<ValidationFilter<CreateGameDTO>>()
                .RequireAuthorization("admin");

            group.MapGet("/", GetAllGames)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("games-get"))
                .Pagination();


            group.MapGet("/{id:int}", GetGameById);
            group.MapDelete("/{id:int}", Delete).RequireAuthorization("admin");
            group.MapGet("/get_by_name/{name}", GetGameByName);

            group.MapPut("/{id:int}", Update)
                .DisableAntiforgery()
                .AddEndpointFilter<ValidationFilter<CreateGameDTO>>()
                .RequireAuthorization("admin");

            group.MapPost("/{id:int}/add_genres", AddGenres).RequireAuthorization("admin");
            group.MapPost("/{id:int}/add_rating", AddRating);
            group.MapGet("/filter", GameFilter).Filter();

            return group;
        }

        static async Task<Results<Created<GameDTO>, ValidationProblem>> AddNewGame([FromForm] CreateGameDTO createGameDTO,
            IGameRepository repository, IMapper mapper, IOutputCacheStore outputCacheStore, IImageStorage imageStorage)
        {
            Game game = mapper.Map<Game>(createGameDTO);

            if (createGameDTO.Photo is not null)
            {
                string url = await imageStorage.Store(container, createGameDTO.Photo);
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

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateGameDTO createGameDTO,
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
                string url = await imageStorage.Update(game.Photo, container, createGameDTO.Photo);
                game.Photo = url;
            }


            await repository.Update(game);
            await outputCacheStore.EvictByTagAsync("games-get", default);

            return TypedResults.NoContent();
        }
        static async Task<Ok<List<GameDTO>>> GetAllGames(IGameRepository repository, IMapper mapper,
            PaginationDTO pagination)
        {

            List<Game> game = await repository.GetAllGames(pagination);

            List<GameDTO> gameDto = mapper.Map<List<GameDTO>>(game);
            return TypedResults.Ok(gameDto);
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AddGenres(int id, List<int> genresIds,
            IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            if (!await gameRepository.Exist(id))
            {
                return TypedResults.NotFound();
            }

            var DBGenres = new List<int>();

            if (genresIds.Count != 0)
            {
                DBGenres = await genreRepository.Exist(genresIds);
            }

            if (DBGenres.Count != genresIds.Count)
            {
                var NotFoundGenres = genresIds.Except(DBGenres);

                return TypedResults.BadRequest($"Genres with id {string.Join(",", NotFoundGenres)} not exist.");
            }
            await gameRepository.AddGenres(id, genresIds);
            return TypedResults.NoContent();
        }
        static async Task<Results<NoContent, NotFound>> AddRating(int id, AddRatingDTO addRatingDTO,
            IGameRepository gameRepository, IRatingRepository ratingRepository
            , IMapper mapper, IUserService userService)
        {
            if (!await gameRepository.Exist(id))
            {
                return TypedResults.NotFound();
            }
            var user = await userService.GetUser();

            if (user is null)
            {
                return TypedResults.NotFound();
            }


            var DBRating = await ratingRepository.Exist(user.Id, id);

            if (DBRating is null)
            {
                Rating rating = mapper.Map<Rating>(addRatingDTO);
                rating.GameId = id;
                rating.UserId = user.Id;
                await ratingRepository.Add(rating);

                return TypedResults.NoContent();
            }

            DBRating.Stars = addRatingDTO.Stars;

            await ratingRepository.Save();


            return TypedResults.NoContent();
        }

        static async Task<Ok<List<GameDTO>>> GameFilter(GameFilterDTO gameFilterDTO,
            IGameRepository repository, IMapper mapper)
        {
            var games = await repository.Filter(gameFilterDTO);
            var gamesDTO = mapper.Map<List<GameDTO>>(games);
            return TypedResults.Ok(gamesDTO);
        }

    }
}
