using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // vice versa bcoz we need to map a to b and b to a.
            // the below one is a time consuming method 
            CreateMap<VillaDTO, VillaCreateDTO>();
            CreateMap<VillaDTO, VillaUpdateDTO>();


            // Automappings for villa number
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

        }
    }
}
