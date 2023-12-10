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
        }
    }
}
