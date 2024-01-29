using BoatSea.Data;
using BoatSea.DTOs;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoatSea.Services
{
    public class RentService : IRentService
    {
        private readonly DatabaseContext _databaseContext;
        public RentService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CancelRent(int id)
        {
            var rent = await _databaseContext.Rents.FindAsync(id);

            if (rent != null)
            {
                var boat = await _databaseContext.Boats.FindAsync(rent.BoatId);
                if (boat != null)
                {
                    boat.Available = true;
                    _databaseContext.Boats.Update(boat);
                }

                _databaseContext.Rents.Remove(rent);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<List<RentDetailsDTO>> GetAllRentsByUser(int userId)
        {
            return await _databaseContext.Rents
            .Where(r => r.UserId == userId)
            .Join(_databaseContext.Boats,
              rent => rent.BoatId,
              boat => boat.Id,
              (rent, boat) => new RentDetailsDTO
              {
                  Id = rent.Id,
                  BoatId = boat.Id,
                  Name = boat.Name,
                  StartDate = rent.DatumIznajmljivanja,
                  EndDate = rent.DatumKrajaIznajmljivanja,
                  ImageName = boat.ImageName,
                  Type = boat.Type,
              })
        .ToListAsync();
        }

        public async Task RentABoat(Rent rent)
        {
            await _databaseContext.Rents.AddAsync(rent);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
