using System.ComponentModel.DataAnnotations.Schema;

namespace GameRaitingAPI.Entitie
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? Photo { get; set; }
        public List<GameGenres> GameGenres { get; set; } = new List<GameGenres>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        [NotMapped]
        public double AverageRating =>
            Ratings.Any() ? Ratings.Average(r => r.Stars) : 0;
        [NotMapped] 
        public int TotalRatings => Ratings.Count;
    }
}
