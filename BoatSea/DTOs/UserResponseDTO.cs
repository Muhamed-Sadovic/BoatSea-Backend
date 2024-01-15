using BoatSea.Models;

namespace BoatSea.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
