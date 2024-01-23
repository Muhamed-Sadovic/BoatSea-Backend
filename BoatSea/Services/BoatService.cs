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

        public async Task CreateBoat(Boat boat)
        {
            await _databaseContext.Boats.AddAsync(boat);
            await _databaseContext.SaveChangesAsync();
        }
        public async Task DeleteBoatAsync(Boat boat)
        {
            _databaseContext.Boats.Remove(boat);
            await _databaseContext.SaveChangesAsync();
        }
        public async Task<List<Boat>> GetAllBoatsAsync()
        {
            return await _databaseContext.Boats.ToListAsync();
        }
        public async Task<List<Boat>> GetByAvailable()
        {
            return await _databaseContext.Boats.Where(x => x.Available).ToListAsync();
        }

        public async Task<List<Boat>> GetByType(string type)
        {
            return await _databaseContext.Boats.Where(x => x.Type == type).ToListAsync();
        }

        public async Task<Boat> GetByIdAsync(int id)
        {
            return await _databaseContext.Boats.Where(b => b.Id == id).FirstOrDefaultAsync();
        }
        public async Task UpdateBoatAsync(Boat boat)
        {
            _databaseContext.Update(boat);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task UpdateAvailable(int id)
        {
            var boat = await _databaseContext.Boats.FirstOrDefaultAsync(b => b.Id == id);

            boat.Available = false;
            _databaseContext.Boats.Update(boat);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
