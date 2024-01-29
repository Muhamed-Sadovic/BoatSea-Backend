using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IBoatService
    {
        Task<List<Boat>> GetAllBoatsAsync();
        Task<Boat> GetByIdAsync(int id);
        Task CreateBoat(Boat boat);
        Task UpdateBoatAsync(Boat boat);
        Task DeleteBoatAsync(Boat boat);
        Task<List<Boat>> GetByAvailable(); //da li su slobodni
        Task<List<Boat>> GetByType(string type);
        Task UpdateAvailable(int id);
        Task UpdateAvailableTrue(int id);
    }
}
