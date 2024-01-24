using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.DTOs
{
    public class UpdateUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
    }
}
