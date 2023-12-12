using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int id);
        Task DeleteUser(User user);
    }
}
