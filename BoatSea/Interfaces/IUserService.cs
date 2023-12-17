using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int id);
        Task DeleteUser(User user);
        string GenerateToken(User user);
        string HashPassword(string password);
        Task AddUserToRole(User user);
        Task RegisterUser(User user);
        Task CreateRole(UserRole role);
        Task<User?> GetUserByEmail(string email);
    }
}
