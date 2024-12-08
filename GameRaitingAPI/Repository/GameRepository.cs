using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;
using GameRaitingAPI.Repository.IRepository;
using GameRaitingAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameRaitingAPI.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        private readonly HttpContext _httpContext;
        public GameRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;

        }
        public async Task<int> Add(Game game)
        {
            _context.Add(game);
            await _context.SaveChangesAsync();
            return game.Id;
        }

        public async Task Delete(int id)
        {
            await _context.games.Where(a => a.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.games.AnyAsync(a => a.Id == id);
        }

        public async Task<List<int>> Exist(List<int> ids)
        {
            return await _context.games.Where(a => ids.Contains(a.Id)).Select(a => a.Id).ToListAsync();
        }

        public async Task<Game?> GetGameById(int id)
        {
            return await _context.games.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Game>> GetGameByName(string name)
        {
            return await _context.games
                .Where(a => a.Name.Contains(name))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<List<Game>> GetAllGames(PaginationDTO paginationDTO)
        {
            var queryable = _context.games.AsQueryable();
            await _httpContext.InsertCountGamesHeader(queryable);
            return await queryable.OrderBy(a => a.Name).Pagination(paginationDTO).ToListAsync();
        }

        public async Task Update(Game game)
        {
            _context.Update(game);
            await _context.SaveChangesAsync();
        }
    }
}
