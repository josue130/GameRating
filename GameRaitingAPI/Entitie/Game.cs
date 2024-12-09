namespace GameRaitingAPI.Entitie
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? Photo { get; set; }
        public List<GameGenres> GameGenres { get; set; } = new List<GameGenres>();
        
    }
}
