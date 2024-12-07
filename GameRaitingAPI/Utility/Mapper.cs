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
        }
    }
}
