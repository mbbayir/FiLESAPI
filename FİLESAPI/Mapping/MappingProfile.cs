using AutoMapper;
using FİLESAPI.Dtos;
using FİLESAPI.Models;

namespace FİLESAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            

     
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<Fillies,FilliesDto>().ReverseMap();   
            CreateMap<Folder, FolderDto>().ReverseMap();
            CreateMap<Folder, Folderfilles>()
                .ForMember(opt=>opt.Fillies, opt=>opt.MapFrom(path=>path.Files))
                .ReverseMap();
            


        }
    }
}


