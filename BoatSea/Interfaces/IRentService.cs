using BoatSea.DTOs;
using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IRentService
    {
        Task RentABoat(Rent rent);
        Task <List<RentDetailsDTO>> GetAllRentsByUser(int userId);
        Task CancelRent(int id);
    }
}
