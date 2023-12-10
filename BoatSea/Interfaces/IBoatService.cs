using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IBoatService
    {
        Task<List<Boat>> GetAllBoatsAsync();
        Task<Boat> GetByIdAsync(int id);
        Task DeleteBoat(Boat boat);
    }
}
