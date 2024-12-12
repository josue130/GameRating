using AutoMapper;
using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;
using GameRaitingAPI.Repository.IRepository;
using GameRaitingAPI.Services;
using GameRaitingAPI.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace GameRaitingAPI.Endpoints
{
    public static class CommentEndpoints
    {
        public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll)
                 .CacheOutput(c =>
                 c.Expire(TimeSpan.FromSeconds(60))
                 .Tag("get-comments")
                 .SetVaryByRouteValue(new string[] { "gameId" }));
            group.MapGet("/{id:int}", GetbyId);

            group.MapPost("/", Create).RequireAuthorization();

            group.MapPut("/{id:int}", Update)
                .RequireAuthorization();

            group.MapDelete("/{id:int}", Delete).RequireAuthorization();
            return group;
        }
        static async Task<Results<Created<CommentDTO>, NotFound, BadRequest<string>>> Create(int gameId,
            CreateCommentDTO createCommentDTO, ICommentRepository commentRepository,
            IGameRepository gameRepository, IMapper mapper, IOutputCacheStore outputCacheStore,
            IUserService userService)
        {
            if (!await gameRepository.Exist(gameId))
            {
                return TypedResults.NotFound();
            }

            var comment = mapper.Map<Comment>(createCommentDTO);
            comment.GameId = gameId;

            var user = await userService.GetUser();

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            comment.UserId = user.Id;

            var id = await commentRepository.Add(comment);
            await outputCacheStore.EvictByTagAsync("get-comments", default);
            var comentarioDTO = mapper.Map<CommentDTO>(comment);
            return TypedResults.Created($"/comment/{id}", comentarioDTO);
        }

        static async Task<Results<Ok<List<CommentDTO>>, NotFound>> GetAll(int gameId,
            ICommentRepository commentRepository, IGameRepository gameRepository,
            IMapper mapper)
        {
            if (!await gameRepository.Exist(gameId))
            {
                return TypedResults.NotFound();
            }

            var comments = await commentRepository.GetAllComments(gameId);
            var commentsDTO = mapper.Map<List<CommentDTO>>(comments);
            return TypedResults.Ok(commentsDTO);
        }
        static async Task<Results<Ok<CommentDTO>, NotFound>> GetbyId(int id,
            ICommentRepository commentRepository, IGameRepository gameRepository,
            IMapper mapper)
        {
            
            var comment = await commentRepository.GetCommentById(id);

            if (comment is null)
            {
                return TypedResults.NotFound();
            }

            var commentDTO = mapper.Map<CommentDTO>(comment);
            return TypedResults.Ok(commentDTO);
        }


        static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Delete(int gameId, int id,
            ICommentRepository commentRepository, IOutputCacheStore outputCacheStore,
            IUserService userService)
        {
            var commentFromDb = await commentRepository.GetCommentById(id);

            if (commentFromDb is null)
            {
                return TypedResults.NotFound();
            }

            var user = await userService.GetUser();

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            if (commentFromDb.UserId != user.Id)
            {
                return TypedResults.Forbid();
            }

            await commentRepository.Delete(id);
            await outputCacheStore.EvictByTagAsync("get-comments", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent, ForbidHttpResult>> Update(int gameId,int id,
            CreateCommentDTO createCommentDTO, ICommentRepository commentRepository,
            IGameRepository gameRepository, IMapper mapper, IOutputCacheStore outputCacheStore,
            IUserService userService)
        {
            if (!await gameRepository.Exist(gameId))
            {
                return TypedResults.NotFound();
            }

            var commentFromDb = await commentRepository.GetCommentById(id);

            if (commentFromDb is null)
            {
                return TypedResults.NotFound();
            }

            var user = await userService.GetUser();

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            if (commentFromDb.UserId != user.Id)
            {
                return TypedResults.Forbid();
            }

            commentFromDb.Message = createCommentDTO.Message;

            await commentRepository.Update(commentFromDb);
            await outputCacheStore.EvictByTagAsync("get-comments", default);
            return TypedResults.NoContent();
        }


    }
}
