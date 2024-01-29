using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.DTOs
{
    public class RegisterUserRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public IFormFile Image {  get; set; }
        public string ImageName { get; set; }
        public string Role { get; set; }
    }
}
