﻿namespace GameRaitingAPI.Entitie
{
    public class GameGenres
    {
        public int GameId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
