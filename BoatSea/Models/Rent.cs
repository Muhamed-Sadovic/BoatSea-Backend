    using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? BoatId { get; set; }
        [ForeignKey("BoatId")]
        public Boat Boat { get; set; }
        public DateTime DatumIznajmljivanja { get; set; }
        public DateTime DatumKrajaIznajmljivanja { get; set; }
    }
}
