namespace GameRatingAPI.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? Photo { get; set; }
        public List<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }

    }
}
