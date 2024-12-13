using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;

namespace GameRaitingAPI.Repository.IRepository
{
    public interface IGameRepository
    {
        Task Update(Game game);
        Task Delete(int id);
        Task<int> Add(Game game);
        Task<bool> Exist(int id);
        Task<List<int>> Exist(List<int> ids);
        Task<Game?> GetGameById(int id);
        Task<List<Game>> GetGameByName(string name);
        Task<List<Game>> GetAllGames(PaginationDTO paginationDTO);
        Task AddGenres(int id, List<int> genresIds);
        Task<List<Game>> Filter(GameFilterDTO gameFilterDTO);
    }
}
