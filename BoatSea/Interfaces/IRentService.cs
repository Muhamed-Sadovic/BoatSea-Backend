using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IRentService
    {
        Task RentABoat(Rent rent);

    }
}
