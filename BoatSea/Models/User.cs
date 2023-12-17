﻿namespace BoatSea.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public List<UserRole> Roles { get; set; }
        public ICollection<Rent> Rents { get; set; }


    }
}
