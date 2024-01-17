using AutoMapper;
using BoatSea.DTOs;
using BoatSea.Models;

namespace BoatSea.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User,UserResponseDTO>();
            CreateMap<UserResponseDTO,User>();
            CreateMap<RegisterUserRequestDTO, User>();

            CreateMap<Boat,BoatResponseDTO>();
            CreateMap<BoatRequestDTO,Boat>();
            CreateMap<BoatResponseDTO,Boat>();
        }
    }
}
