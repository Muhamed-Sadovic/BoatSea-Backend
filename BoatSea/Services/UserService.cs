using BoatSea.Data;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatSea.Services
{
    public class UserService : IUserService
    {
        public DatabaseContext _databaseContext { get; set; }
        public UserService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task DeleteUser(User user)
        {
            _databaseContext.Users.Remove(user);
             await _databaseContext.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _databaseContext.Users.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}
