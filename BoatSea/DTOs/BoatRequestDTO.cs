using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.DTOs
{
    public class BoatRequestDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }
}
