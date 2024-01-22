namespace BoatSea.DTOs
{
    public class RentResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BoatId { get; set; }
        public DateTime DatumIznajmljivanja { get; set; }
        public DateTime DatumKrajaIznajmljivanja { get; set; }
    }
}
