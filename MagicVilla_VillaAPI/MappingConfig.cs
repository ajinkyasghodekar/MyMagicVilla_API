using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // vice versa bcoz we need to map a to b and b to a.
            // the below one is a time consuming method 
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            // shortcut way to map and also reverse map
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();

            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();



            // Automappings for villa number
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

        }
    }
}
