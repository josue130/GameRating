using GameRaitingAPI.Entitie;

namespace GameRaitingAPI.Repository.IRepository
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAll();
        Task<Genre?> GetById(int id);
        Task<int> Add(Genre genre);
        Task Update (Genre genre);
        Task Delete(int id);
      
    }
}
