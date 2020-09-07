using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;

namespace ParkyAPI.Mapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<NationalPark, NationalParkDtoNoPhoto>().ReverseMap();
            CreateMap<Trail, TrailDto>()
                .ReverseMap();
            CreateMap<CreateTrailDto, Trail>().ForMember(m => m.Id, opt =>
                {
                    opt.Ignore();
                }).ReverseMap();

            CreateMap<UpdateTrailDto, Trail>()
                .ReverseMap();
        }
    }
}
