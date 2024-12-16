using GameRatingAPI;
using GameRatingAPI.Entitie;
using GameRatingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GameRatingAPI.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<int> Add(Rating rating)
        {
            var game = await _context.games.FirstOrDefaultAsync(g => g.Id == rating.GameId);

            if (game is null)
            {
                throw new ArgumentException($"Game with {rating.Id} not found");
            }

            await _context.AddAsync(rating);

            await Save();


            return rating.Id;
        }
        public async Task<Rating?> Exist(string userId, int gameId)
        {
            return await _context.ratings.FirstOrDefaultAsync(r => r.GameId == gameId && r.UserId == userId);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
