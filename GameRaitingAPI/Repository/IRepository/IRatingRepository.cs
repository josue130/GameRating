using GameRaitingAPI.Entitie;

namespace GameRaitingAPI.Repository.IRepository
{
    public interface IRatingRepository
    {
        Task<int> Add(Rating rating);
        Task<Rating?> Exist(string userId, int gameId);
        Task Save();

    }
}
