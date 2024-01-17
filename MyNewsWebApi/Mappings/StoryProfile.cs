using AutoMapper;
using MyNewsWebApi.Entities;
using MyNewsWebApi.Models;

namespace MyNewsWebApi.Mappings
{
    public class StoryProfile : Profile
    {
        public StoryProfile()
        {
            CreateMap<Story, StoryDto>()
                .ForMember(dest => dest.Time, 
                    opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.Time).LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ss")))
                .ForMember(dest => dest.PostedBy,
                    opt => opt.MapFrom(src => src.By))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Kids.Length));
        }
    }
}
