﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BoatSea.Models
{
    public class Boat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<Rent> Rents { get; set; }
    }
}
