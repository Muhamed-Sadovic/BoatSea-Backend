using BoatSea.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.DTOs
{
    public class RentRequestDTO
    {
        public int UserId { get; set; }
        public int BoatId { get; set; }
        public DateTime DatumIznajmljivanja { get; set; }
        public DateTime DatumKrajaIznajmljivanja { get; set; }
    }
}
