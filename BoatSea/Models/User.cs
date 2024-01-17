using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";
        [NotMapped]
        public IFormFile Image {  get; set; }
        public string ImageName { get; set; }
        public ICollection<Rent> Rents { get; set; }


    }
}
