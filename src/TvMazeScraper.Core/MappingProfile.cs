using AutoMapper;
using TvMazeScraper.Core.Dtos;
using TvMazeScraper.Core.Entities;

namespace TvMazeScraper.Api.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cast, CastDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.PersonId)
            ).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Person.Name)
            ).ForMember(
                dest => dest.BirthDay,
                opt => opt.MapFrom(src => src.Person.BirthDay)
            );

            CreateMap<Show, ShowDto>();


        }
    }
}
