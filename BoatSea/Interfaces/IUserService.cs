using BoatSea.Models;

namespace BoatSea.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int id);
        Task DeleteUser(User user);
        Task UpdateUserAsync(User user);
        string GenerateToken(User user);
        string HashPassword(string password);
        string CreateRandomToken();
        string GenerateResetToken(User user);
        Task RegisterUser(User user);
        Task<User?> GetUserByEmail(string email);
        Task SendEmailAsync(string to, string subject, string htmlContent);
        Task SendPasswordResetEmail(string email, string resetToken);
        Task<User> GetUserByResetToken(string token);
        Task ResetPassword(User user, string newPassword);

        Task<List<Drzava>> GetGrad(string drzava);
    }
}
