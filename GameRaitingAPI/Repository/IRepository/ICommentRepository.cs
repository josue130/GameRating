using GameRatingAPI.Entitie;

namespace GameRatingAPI.Repository.IRepository
{
    public interface ICommentRepository
    {
        Task Update(Comment comment);
        Task Delete(int id);
        Task<int> Add(Comment comment);
        Task<bool> Exist(int id);
        Task<Comment?> GetCommentById(int id);
        Task<List<Comment>> GetAllComments(int gameId);

    }
}
