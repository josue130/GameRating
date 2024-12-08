namespace GameRaitingAPI.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? Photo { get; set; }
    }
}
