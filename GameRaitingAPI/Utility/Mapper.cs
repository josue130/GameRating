using AutoMapper;
using GameRaitingAPI.DTOs;
using GameRaitingAPI.Entitie;

namespace GameRaitingAPI.Utility
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<CreateGenreDTO, Genre>().ReverseMap();

            CreateMap<CreateGameDTO, Game>()
                .ForMember(x => x.Photo, option => option.Ignore()).ReverseMap();
            CreateMap<Game, GameDTO>()
                .ForMember(g => g.Genres,
                entitie => entitie.MapFrom(ga => ga.GameGenres.Select(gg =>  new GenreDTO {Id = gg.GenreId
                ,Name = gg.Genre.Name})))
                .ReverseMap();
        }
    }
}
