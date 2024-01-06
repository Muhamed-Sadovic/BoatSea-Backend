namespace BoatSea.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserRole> Roles { get; set; }
        public ICollection<Rent> Rents { get; set; }


    }
}
