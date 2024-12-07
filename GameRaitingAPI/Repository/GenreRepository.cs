using GameRaitingAPI.Entitie;
using GameRaitingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GameRaitingAPI.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext context;

        public GenreRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }

        public async Task Delete(int id)
        {
            await context.genres.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exist(int id)
        {
            return await context.genres.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Exist(int id, string name)
        {
            return await context.genres.AnyAsync(x => x.Id != id && x.Name == name);
        }

        public async Task<List<Genre>> GetAll()
        {
            return await context.genres.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await context.genres.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Genre genre)
        {
            context.Update(genre);
            await context.SaveChangesAsync();       
        }
    }
}
