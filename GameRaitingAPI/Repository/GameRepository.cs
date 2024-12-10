using AutoMapper;
using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;
using GameRaitingAPI.Repository.IRepository;
using GameRaitingAPI.Utility;
using Microsoft.EntityFrameworkCore;


namespace GameRaitingAPI.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;
        public GameRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {

            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;
            _mapper = mapper;

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
            return await _context.games
                .Include(g => g.GameGenres)
                    .ThenInclude(gp => gp.Genre)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
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
        public async Task AddGenres(int id, List<int> genresIds)
        {
            var game = await _context.games.Include(g => g.GameGenres).FirstOrDefaultAsync(p => p.Id == id);

            if (game is null)
            {
                throw new ArgumentException($"Game with {id} not found");
            }

            var gamesGenres = genresIds.Select(genreId => new GameGenres() { GenreId = genreId });

            game.GameGenres = _mapper.Map(gamesGenres, game.GameGenres);

            await _context.SaveChangesAsync();
        }
    }
}
