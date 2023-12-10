using BoatSea.Data;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatSea.Services
{
    public class BoatService : IBoatService
    {
        private readonly DatabaseContext _databaseContext;
        public BoatService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task DeleteBoat(Boat boat)
        {
            _databaseContext.Boats.Remove(boat);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Boat>> GetAllBoatsAsync()
        {
            return await _databaseContext.Boats.ToListAsync();
        }

        public async Task<Boat> GetByIdAsync(int id)
        {
            return await _databaseContext.Boats.Where(b => b.Id == id).FirstOrDefaultAsync();
        }
    }
}
