using BoatSea.Data;
using BoatSea.Interfaces;
using BoatSea.Models;

namespace BoatSea.Services
{
    public class RentService : IRentService
    {
        private readonly DatabaseContext _databaseContext;
        public RentService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task RentABoat(Rent rent)
        {
            await _databaseContext.Rents.AddAsync(rent);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
