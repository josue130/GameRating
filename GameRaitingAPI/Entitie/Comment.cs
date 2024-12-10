using Microsoft.AspNetCore.Identity;

namespace GameRaitingAPI.Entitie
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public int GameId { get; set; }
        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;
        public Game Game { get; set; } = null!;
        
    }
}
