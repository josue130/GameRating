﻿namespace GameRatingAPI.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
