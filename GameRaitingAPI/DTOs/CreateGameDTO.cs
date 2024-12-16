namespace GameRatingAPI.DTOs
{
    public class CreateGameDTO
    {
        public string Name { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
