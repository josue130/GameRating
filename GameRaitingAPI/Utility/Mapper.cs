using AutoMapper;
using GameRatingAPI.DTOs;
using GameRatingAPI.Entitie;

namespace GameRatingAPI.Utility
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
                entitie => entitie.MapFrom(ga => ga.GameGenres.Select(gg => new GenreDTO
                {
                    Id = gg.GenreId
                ,
                    Name = gg.Genre.Name
                })))
                .ForMember(dest => dest.AverageRating,
                opt => opt.MapFrom(src => src.Ratings.Any() ? src.Ratings.Average(r => r.Stars) : 0))
            .ForMember(dest => dest.TotalRatings,
                opt => opt.MapFrom(src => src.Ratings.Count))
                .ReverseMap();
            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<Rating, AddRatingDTO>().ReverseMap();
        }
    }
}
