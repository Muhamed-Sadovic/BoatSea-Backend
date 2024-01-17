using BoatSea.Models;

namespace BoatSea.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageName {  get; set; }
        public string Role { get; set; } = "User";
    }
}
