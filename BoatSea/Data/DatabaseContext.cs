using BoatSea.Models;
using Microsoft.EntityFrameworkCore;

namespace BoatSea.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Boat> Boats { get; set; }
        //public DbSet<Rent> Rents { get; set; }

    }
}
