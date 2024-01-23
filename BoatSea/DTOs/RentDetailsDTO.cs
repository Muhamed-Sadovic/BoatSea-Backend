namespace BoatSea.DTOs
{
    public class RentDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }
    }
}
