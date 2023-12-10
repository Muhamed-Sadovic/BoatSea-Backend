using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Models;

namespace BoatSea.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User,UserDTO>();
            CreateMap<UserDTO,User>();

            CreateMap<Boat,BoatDTO>();
            CreateMap<BoatDTO,Boat>();
        }
    }
}
