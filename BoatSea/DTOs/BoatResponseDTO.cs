using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.DTOs
{
    public class BoatResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; } = true;
        public string Description { get; set; }
        public string ImageName { get; set; }
    }
}
