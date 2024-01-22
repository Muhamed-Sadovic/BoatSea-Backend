using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.Models
{
    public class Boat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; } = true;
        [NotMapped]
        public IFormFile Image { get; set; }    
        public string ImageName { get; set; }
        public string Description { get; set; }
        public ICollection<Rent> Rents { get; set; }
    }
    public class CheckoutRequest
    {
        public int Price { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
