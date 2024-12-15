﻿using GameRaitingAPI.Entitie;

namespace GameRaitingAPI.GraphQL
{
    public class Query
    {
        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Game> GetGames([Service] AppDbContext context) => context.games;

        [Serial]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Genre> GetGenres([Service] AppDbContext context) => context.genres;
    }
}
