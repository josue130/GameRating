using Microsoft.AspNetCore.Identity;

namespace GameRatingAPI.Entitie
{
    public class Rating
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
