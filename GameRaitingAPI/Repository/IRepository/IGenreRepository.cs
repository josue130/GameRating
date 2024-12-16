using GameRatingAPI.Entitie;

namespace GameRatingAPI.Repository.IRepository
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAll();
        Task<Genre?> GetById(int id);
        Task<int> Add(Genre genre);
        Task Update(Genre genre);
        Task Delete(int id);
        Task<bool> Exist(int id);
        Task<bool> Exist(int id, string name);
        Task<List<int>> Exist(List<int> ids);

    }
}
