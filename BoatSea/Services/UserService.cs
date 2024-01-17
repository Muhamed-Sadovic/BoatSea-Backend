using BoatSea.Data;
using BoatSea.Interfaces;
using BoatSea.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BoatSea.Services
{
    public class UserService : IUserService
    {
        public DatabaseContext _databaseContext { get; set; }
        private readonly IConfiguration _configuration;
        public UserService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
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

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Auth:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public string HashPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
        public async Task RegisterUser(User user)
        {
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _databaseContext.Users.Where(e =>  e.Email == email).FirstOrDefaultAsync();
        }
    }
}
