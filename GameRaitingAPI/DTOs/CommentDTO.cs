namespace GameRatingAPI.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public int GameId { get; set; }
    }
}
